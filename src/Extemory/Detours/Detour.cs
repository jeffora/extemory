using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Extemory.Detours
{
    public class Detour<T> : IDisposable where T : class
    {
        private readonly List<byte> _original;
        private readonly List<byte> _new;
        private readonly T _targetDelegate;
        private readonly IntPtr _targetAddr;
        private readonly IntPtr _hookAddr;

        internal Detour(IntPtr targetAddr, T detourFunc)
        {
            if (typeof(T).GetCustomAttributes(typeof(UnmanagedFunctionPointerAttribute), true).Length == 0)
            {
                throw new ArgumentException("Type T must be a delegate type adorned with the UnmanagedFunctionPointer attribute");
            }

            _targetAddr = targetAddr;
            _targetDelegate = _targetAddr.ToDelegate<T>();
            _hookAddr = ((Delegate)(object)detourFunc).ToFunctionPointer();

            // store the original function starting bytes
            _original = new List<byte>();
            _original.AddRange(_targetAddr.ReadArray<byte>(6));

            _new = new List<byte> { 0x68 };
            _new.AddRange(BitConverter.GetBytes(_hookAddr.ToInt32()));
            _new.Add(0xC3);
        }

        public bool IsApplied { get; private set; }

        public bool Apply()
        {
            try
            {
                _targetAddr.WriteArray(_new.ToArray());
                IsApplied = true;
                return true;
            }
            catch (Exception)
            {
                // TODO: exeption handling
                return false;
            }
        }

        public bool Remove()
        {
            try
            {
                _targetAddr.WriteArray(_original.ToArray());
                IsApplied = false;
                return true;
            }
            catch (Exception)
            {
                // TODO: exception handling
                return false;
            }
        }

        public object CallOriginal(params object[] args)
        {
            Remove();
            var ret = ((Delegate)(object)_targetDelegate).DynamicInvoke(args);
            Apply();
            return ret;
        }

        #region IDisposable
        
        public void Dispose()
        {
            if (IsApplied)
            {
                Remove();
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
