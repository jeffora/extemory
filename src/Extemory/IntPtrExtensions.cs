using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using Extemory.Disassembly;
using Extemory.MemoryEdits;
using System.Diagnostics;

using Extemory.Win32;

namespace Extemory
{
    public static class IntPtrExtensions
    {
        private const int MaxStringSizeBytes = 1024;

        /// <summary>
        /// Read a struct type from an unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">Struct type to read</typeparam>
        /// <param name="addr">Pointer address to read from</param>
        /// <returns></returns>
        public static unsafe T Read<T>(this IntPtr addr) where T : struct
        {
            var pAddr = addr.ToPointer();

            object ret;

            if (typeof(T) == typeof(IntPtr))
            {
                ret = new IntPtr(*(void**)pAddr);
                return (T)ret;
            }

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    ret = *(bool*)pAddr;
                    break;
                case TypeCode.Byte:
                    ret = *(byte*)pAddr;
                    break;
                case TypeCode.SByte:
                    ret = *(sbyte*)pAddr;
                    break;
                case TypeCode.Char:
                    ret = *(char*)pAddr;
                    break;
                case TypeCode.Int16:
                    ret = *(short*)pAddr;
                    break;
                case TypeCode.UInt16:
                    ret = *(ushort*)pAddr;
                    break;
                case TypeCode.Int32:
                    ret = *(int*)pAddr;
                    break;
                case TypeCode.UInt32:
                    ret = *(uint*)pAddr;
                    break;
                case TypeCode.Int64:
                    ret = *(long*)pAddr;
                    break;
                case TypeCode.UInt64:
                    ret = *(ulong*)pAddr;
                    break;
                case TypeCode.Single:
                    ret = *(float*)pAddr;
                    break;
                case TypeCode.Double:
                    ret = *(double*)pAddr;
                    break;
                default:
                    // assume a custom struct, lets try
                    ret = Marshal.PtrToStructure(addr, typeof(T));
                    break;
            }

            return (T)ret;
        }

        /// <summary>
        /// Write a structure to unmanaged memory
        /// </summary>
        /// <typeparam name="T">Struct type to write</typeparam>
        /// <param name="addr">Address to write to</param>
        /// <param name="data">Struct data to write</param>
        public static unsafe void Write<T>(this IntPtr addr, T data) where T : struct
        {
            var pAddr = addr.ToPointer();

            object oData = data;

            if (typeof(T) == typeof(IntPtr))
            {
                *((void**)pAddr) = ((IntPtr)oData).ToPointer();
            }

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Boolean:
                    *(bool*)pAddr = (bool)oData;
                    break;
                case TypeCode.Byte:
                    *(byte*)pAddr = (byte)oData;
                    break;
                case TypeCode.SByte:
                    *(sbyte*)pAddr = (sbyte)oData;
                    break;
                case TypeCode.Char:
                    *(char*)pAddr = (char)oData;
                    break;
                case TypeCode.Int16:
                    *(short*)pAddr = (short)oData;
                    break;
                case TypeCode.UInt16:
                    *(ushort*)pAddr = (ushort)oData;
                    break;
                case TypeCode.Int32:
                    *(int*)pAddr = (int)oData;
                    break;
                case TypeCode.UInt32:
                    *(uint*)pAddr = (uint)oData;
                    break;
                case TypeCode.Int64:
                    *(long*)pAddr = (long)oData;
                    break;
                case TypeCode.UInt64:
                    *(ulong*)pAddr = (ulong)oData;
                    break;
                case TypeCode.Single:
                    *(float*)pAddr = (float)oData;
                    break;
                case TypeCode.Double:
                    *(double*)pAddr = (double)oData;
                    break;
                default:
                    // assume a custom struct, lets try
                    Marshal.StructureToPtr(oData, addr, true);
                    break;
            }

        }

        /// <summary>
        /// Read an array of integral types (int, float, byte, etc) from unmanaged memory.
        /// </summary>
        /// <typeparam name="T">Integral type to read. Must be struct, but not all structs are supported (only those supported by Marshal.Copy</typeparam>
        /// <param name="addr">Address to read array from</param>
        /// <param name="size">Size of the array to read (number of elements)</param>
        /// <returns></returns>
        public static T[] ReadArray<T>(this IntPtr addr, int size) where T : struct
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Byte:
                    var bytes = new byte[size];
                    Marshal.Copy(addr, bytes, 0, size);
                    return bytes.Cast<T>().ToArray();
                case TypeCode.Char:
                    var chars = new char[size];
                    Marshal.Copy(addr, chars, 0, size);
                    return chars.Cast<T>().ToArray();
                case TypeCode.Int16:
                    var shorts = new short[size];
                    Marshal.Copy(addr, shorts, 0, size);
                    return shorts.Cast<T>().ToArray();
                case TypeCode.Int32:
                    var ints = new int[size];
                    Marshal.Copy(addr, ints, 0, size);
                    return ints.Cast<T>().ToArray();
                case TypeCode.Int64:
                    var longs = new long[size];
                    Marshal.Copy(addr, longs, 0, size);
                    return longs.Cast<T>().ToArray();
                case TypeCode.Single:
                    var floats = new float[size];
                    Marshal.Copy(addr, floats, 0, size);
                    return floats.Cast<T>().ToArray();
                case TypeCode.Double:
                    var doubles = new double[size];
                    Marshal.Copy(addr, doubles, 0, size);
                    return doubles.Cast<T>().ToArray();
                default:
                    throw new ArgumentException(string.Format("Unsupported type argument supplied: {0}", typeof(T).Name));
            }
        }

        /// <summary>
        /// Write an array of integral types (int, float, byte, etc) to unmanaged memory.
        /// </summary>
        /// <typeparam name="T">Integral type to write. Must be struct, but not all structs are supported (only those supported by Marshal.Copy</typeparam>
        /// <param name="addr">Address to write array to</param>
        /// <param name="data">Array data to write</param>
        public static void WriteArray<T>(this IntPtr addr, T[] data) where T : struct
        {
            var size = data.Length * Marshal.SizeOf(typeof(T));

            IntPtr temp = Marshal.AllocHGlobal(size);
            try
            {
                switch (Type.GetTypeCode(typeof(T)))
                {
                    case TypeCode.Byte:
                        var bytes = data.Cast<byte>().ToArray();
                        Marshal.Copy(bytes, 0, temp, data.Length);
                        break;
                    case TypeCode.Char:
                        var chars = data.Cast<char>().ToArray();
                        Marshal.Copy(chars, 0, temp, data.Length);
                        break;
                    case TypeCode.Int16:
                        var shorts = data.Cast<short>().ToArray();
                        Marshal.Copy(shorts, 0, temp, data.Length);
                        break;
                    case TypeCode.Int32:
                        var ints = data.Cast<int>().ToArray();
                        Marshal.Copy(ints, 0, temp, data.Length);
                        break;
                    case TypeCode.Int64:
                        var longs = data.Cast<long>().ToArray();
                        Marshal.Copy(longs, 0, temp, data.Length);
                        break;
                    case TypeCode.Single:
                        var floats = data.Cast<float>().ToArray();
                        Marshal.Copy(floats, 0, temp, data.Length);
                        break;
                    case TypeCode.Double:
                        var doubles = data.Cast<double>().ToArray();
                        Marshal.Copy(doubles, 0, temp, data.Length);
                        break;
                    default:
                        throw new ArgumentException(string.Format("Unsupported type argument supplied: {0}", typeof(T).Name));
                }

                int numWritten;
                if (!Imports.WriteProcessMemory(Process.GetCurrentProcess().GetHandle(), addr, temp, (uint)size, out numWritten) || numWritten != size)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            finally
            {
                if (temp != IntPtr.Zero)
                    Marshal.FreeHGlobal(temp);
            }
        }

        /// <summary>
        /// Read a string of the supplied encoding from an unmanaged pointer. This assumes the string is null terminated.
        /// </summary>
        /// <param name="addr">Pointer address to read from</param>
        /// <param name="encoding">Encoding to read</param>
        /// <returns></returns>
        public static string ReadString(this IntPtr addr, Encoding encoding)
        {
            if (encoding.Equals(Encoding.ASCII))
            {
                return Marshal.PtrToStringAnsi(addr);
            }
            if (encoding.Equals(Encoding.Unicode))
            {
                return Marshal.PtrToStringUni(addr);
            }
            return addr.ReadString(encoding, MaxStringSizeBytes);
        }

        /// <summary>
        /// Read a string of the supplied encoding from an unmanaged pointer
        /// </summary>
        /// <param name="addr">Pointer address to read from</param>
        /// <param name="encoding">Encoding to read</param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static string ReadString(this IntPtr addr, Encoding encoding, int maxSize)
        {
            if (!(encoding.Equals(Encoding.UTF8) || encoding.Equals(Encoding.Unicode) || encoding.Equals(Encoding.ASCII)))
            {
                throw new ArgumentException(string.Format("Encoding type {0} is not supported", encoding.EncodingName), "encoding");
            }
            var bytes = addr.ReadArray<byte>(maxSize);
            var terminalCharacterByte = encoding.GetBytes(new[] { '\0' });
            var buffer = new List<byte>();
            for (int i = 0; i < bytes.Length; )
            {
                var match = true;
                var shortBuffer = new List<byte>();
                for (int j = 0; j < terminalCharacterByte.Length; j++)
                {
                    shortBuffer.Add(bytes[i + j]);
                    if (bytes[i + j] != terminalCharacterByte[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    break;
                }
                buffer.AddRange(shortBuffer);
                i += shortBuffer.Count;
                //var range = new byte[terminalCharacterByte.Length];
                //var match = true;
                //for (int j = 0; j < terminalCharacterByte.Length; j++)
                //{
                //    range[j] = bytes[i + j];
                //    if (range[j] != terminalCharacterByte[j]) match = false;
                //}
                //if (!match)
                //{
                //    buffer.AddRange(range);
                //}
                //else
                //{
                //    break;
                //}
            }

            var result = encoding.GetString(buffer.ToArray());
            return result;
        }

        /// <summary>
        /// Write a string of the supplied encoding to an unmanaged pointer
        /// </summary>
        /// <param name="addr">Pointer address to write to</param>
        /// <param name="value">String value to write</param>
        /// <param name="encoding">Encoding to use</param>
        /// <param name="appendNullCharacter">If true, a terminating character for the current encoding will be appended</param>
        public static unsafe void WriteString(this IntPtr addr, string value, Encoding encoding, bool appendNullCharacter = true)
        {
            var bytes = encoding.GetBytes(value);
            if (appendNullCharacter)
            {
                bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            }

            var pDest = (byte*)addr.ToPointer();
            for (int i = 0; i < bytes.Length; i++)
            {
                pDest[i] = bytes[i];
            }
        }

        /// <summary>
        /// Gets a function pointer from an object's virtual method table at the supplied index
        /// </summary>
        /// <param name="addr">Object base address</param>
        /// <param name="index">Virtual method index</param>
        /// <returns></returns>
        public static unsafe IntPtr VTable(this IntPtr addr, int index)
        {
            var pAddr = (void ***)addr.ToPointer();
            return new IntPtr((*pAddr)[index]);
        }

        /// <summary>
        /// Convert a function pointer a managed delegate.
        /// </summary>
        /// <typeparam name="T">A delegate type to convert to. Must be adorned with <see cref="UnmanagedFunctionPointerAttribute"/></typeparam>
        /// <param name="addr">The function pointer</param>
        /// <returns></returns>
        public static T ToDelegate<T>(this IntPtr addr) where T : class
        {
            if (typeof(T).GetCustomAttributes(typeof(UnmanagedFunctionPointerAttribute), true).Length == 0)
            {
                throw new InvalidOperationException("This operation can only convert to delegates adorned with the UnmanagedFunctionPointerAttribute");
            }
            return Marshal.GetDelegateForFunctionPointer(addr, typeof(T)) as T;
        }

        /// <summary>
        /// Detour a function pointer with a managed delegate of type T. The detour is applied immediately
        /// </summary>
        /// <typeparam name="T">Delegate type of detour</typeparam>
        /// <param name="addr">Address of function pointer to detour</param>
        /// <param name="del">Delegate to detour with</param>
        /// <returns></returns>
        public static Detour<T> DetourWith<T>(this IntPtr addr, T del) where T : class
        {
            var manager = MemoryEditManager.ForProcess(Process.GetCurrentProcess());
            var detour = new Detour<T>(addr, del);
            manager[addr] = detour;
            detour.Apply();
            return detour;
        }

        public static IEnumerable<DisassemblyInstruction> Disassemble(this IntPtr pAddr)
        {
            return Disassembler.Disassemble(pAddr);
        }
    }
}