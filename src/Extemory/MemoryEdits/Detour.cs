using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Extemory.Disassembly.Bea;
using Extemory.Win32;
using System.Linq;

namespace Extemory.MemoryEdits
{
    public class Detour<T> : IMemoryEdit where T : class
    {
        private static readonly uint JumpSize = 5; // TODO: x64 support (14 bytes)

        private readonly List<byte> _original;
        private readonly T _targetDelegate;
        private readonly IntPtr _targetAddr;
        private readonly IntPtr _hookAddr;

        private IntPtr _trampoline;

        static Detour()
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)) || typeof(T).GetCustomAttributes(typeof(UnmanagedFunctionPointerAttribute), true).Length == 0)
            {
                throw new ArgumentException("Type T must be a delegate type adorned with the UnmanagedFunctionPointer attribute");
            }
        }

        internal Detour(IntPtr targetAddr, T detourFunc)
        {
            _targetAddr = targetAddr;
            _targetDelegate = _targetAddr.ToDelegate<T>();
            _hookAddr = ((Delegate)(object)detourFunc).ToFunctionPointer();

            // store the original function bytes
            _original = new List<byte>(_targetAddr.ReadArray<byte>((int)JumpSize));
        }

        #region IMemoryEdit
        
        public bool IsApplied { get; private set; }

        public void Apply()
        {

            if (IsApplied)
                return;

            // TODO: handle this situation
            if (_trampoline != IntPtr.Zero)
                throw new InvalidOperationException("Trampoline was not correctly freed");

            _trampoline = Process.GetCurrentProcess()
                .Allocate(IntPtr.Zero, JumpSize * 3, memoryProtection: MemoryProtection.ExecuteReadWrite);
            var pTramp = _trampoline;

            // dissassemble from target address
            // moving JumpSize bytes to trampoline
            var instrByteCount = 0;
            foreach (var instr in _targetAddr.Disassemble())
            {
                // TODO: work out jumps
                pTramp.WriteArray(instr.InstructionData);
                pTramp += instr.Length;
                instrByteCount += instr.Length;

                // We now have enough data, stop disassembling
                if (instrByteCount >= JumpSize)
                    break;
            }

            // write a jmp instruction from trampoline back to function
            WriteJump(pTramp, _targetAddr + instrByteCount);

            // Write jump from target to detour
            WriteJump(_targetAddr, _hookAddr);

            IsApplied = true;
        }

        public void Remove()
        {
            if (!IsApplied)
                return;

            // Restore the original bytes to the function address
            _targetAddr.WriteArray(_original.ToArray());

            // Release the trampoline
            Process.GetCurrentProcess().Free(_trampoline);
        }

        #endregion

        public object CallOriginal(params object[] args)
        {
            if (Debugger.IsAttached)
                Debugger.Break();

            object ret;
            if (IsApplied)
            {
                if (_trampoline == IntPtr.Zero)
                    throw new InvalidOperationException("No trampoline has been allocated - something has gone wrong");

                var tramDel = _trampoline.ToDelegate<T>();
                ret = ((Delegate) (object) tramDel).DynamicInvoke(args);
            }
            else
            {
                ret = ((Delegate) (object) _targetDelegate).DynamicInvoke(args);
            }
            return ret;
        }

        #region IDisposable

        public void Dispose()
        {
            Remove();

            // Ensure _trampoline is freed, even though this should happen in Remove()
            if (_trampoline != IntPtr.Zero)
            {
                Process.GetCurrentProcess().Free(_trampoline);
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        ~Detour()
        {
            Dispose();
        }

        private static void WriteJump(IntPtr pAddr, IntPtr pJmpTarget)
        {
            var jmpBytes = new List<byte>();
            jmpBytes.Add(0xE9);

            // relocate address
            var relocAddr = pJmpTarget.ToInt32() - pAddr.ToInt32() - 5;

            jmpBytes.AddRange(BitConverter.GetBytes(relocAddr));

            pAddr.WriteArray(jmpBytes.ToArray());
        }
    }
}
