using System;
using System.Text;

using Extemory.MemoryEdits;

namespace Extemory
{
    public static class IntegerExtensions
    {
        #region int

        /// <summary>
        /// Read a struct type from an unmanaged pointer.
        /// </summary>
        /// <typeparam name="T">Struct type to read</typeparam>
        /// <param name="addr">Pointer address to read from</param>
        /// <returns></returns>
        public static T Read<T>(this int addr) where T : struct
        {
            return new IntPtr(addr).Read<T>();
        }

        /// <summary>
        /// Write a structure to unmanaged memory
        /// </summary>
        /// <typeparam name="T">Struct type to write</typeparam>
        /// <param name="addr">Address to write to</param>
        /// <param name="data">Struct data to write</param>
        public static void Write<T>(this int addr, T data) where T : struct
        {
            new IntPtr(addr).Write(data);
        }

        /// <summary>
        /// Read an array of integral types (int, float, byte, etc) from unmanaged memory.
        /// </summary>
        /// <typeparam name="T">Integral type to read. Must be struct, but not all structs are supported (only those supported by Marshal.Copy</typeparam>
        /// <param name="addr">Address to read array from</param>
        /// <param name="size">Size of the array to read (number of elements)</param>
        /// <returns></returns>
        public static T[] ReadArray<T>(this int addr, int size) where T : struct
        {
            return new IntPtr(addr).ReadArray<T>(size);
        }

        /// <summary>
        /// Write an array of integral types (int, float, byte, etc) to unmanaged memory.
        /// </summary>
        /// <typeparam name="T">Integral type to write. Must be struct, but not all structs are supported (only those supported by Marshal.Copy</typeparam>
        /// <param name="addr">Address to write array to</param>
        /// <param name="data">Array data to write</param>
        public static void WriteArray<T>(this int addr, T[] data) where T : struct
        {
            new IntPtr(addr).WriteArray(data);
        }


        public static string ReadString(this int addr, Encoding encoding)
        {
            return new IntPtr(addr).ReadString(encoding);
        }

        public static void WriteString(this int addr, string value, Encoding encoding)
        {
            new IntPtr(addr).WriteString(value, encoding);
        }

        public static IntPtr VTable(this int addr, int index)
        {
            return new IntPtr(addr).VTable(index);
        }

        public static T ToDelegate<T>(this int addr) where T : class
        {
            return new IntPtr(addr).ToDelegate<T>();
        }

        public static Detour<T> DetourWith<T>(this int addr, T del) where T : class
        {
            return new IntPtr(addr).DetourWith(del);
        }

        #endregion

        #region uint

        public static T Read<T>(this uint addr) where T : struct
        {
            return new IntPtr(addr).Read<T>();
        }

        public static void Write<T>(this uint addr, T data) where T : struct
        {
            new IntPtr(addr).Write(data);
        }

        public static T[] ReadArray<T>(this uint addr, int size) where T : struct
        {
            return new IntPtr(addr).ReadArray<T>(size);
        }

        public static void WriteArray<T>(this uint addr, T[] data) where T : struct
        {
            new IntPtr(addr).WriteArray(data);
        }

        public static string ReadString(this uint addr, Encoding encoding)
        {
            return new IntPtr(addr).ReadString(encoding);
        }

        public static void WriteString(this uint addr, string value, Encoding encoding)
        {
            new IntPtr(addr).WriteString(value, encoding);
        }

        public static IntPtr VTable(this uint addr, int index)
        {
            return new IntPtr(addr).VTable(index);
        }

        public static T ToDelegate<T>(this uint addr) where T : class
        {
            return new IntPtr(addr).ToDelegate<T>();
        }

        public static Detour<T> DetourWith<T>(this uint addr, T del) where T : class
        {
            return new IntPtr(addr).DetourWith(del);
        }

        #endregion

        #region long

        public static T Read<T>(this long addr) where T : struct
        {
            return new IntPtr(addr).Read<T>();
        }

        public static void Write<T>(this long addr, T data) where T : struct
        {
            new IntPtr(addr).Write(data);
        }

        public static T[] ReadArray<T>(this long addr, int size) where T : struct
        {
            return new IntPtr(addr).ReadArray<T>(size);
        }

        public static void WriteArray<T>(this long addr, T[] data) where T : struct
        {
            new IntPtr(addr).WriteArray(data);
        }

        public static string ReadString(this long addr, Encoding encoding)
        {
            return new IntPtr(addr).ReadString(encoding);
        }

        public static void WriteString(this long addr, string value, Encoding encoding)
        {
            new IntPtr(addr).WriteString(value, encoding);
        }

        public static IntPtr VTable(this long addr, int index)
        {
            return new IntPtr(addr).VTable(index);
        }

        public static T ToDelegate<T>(this long addr) where T : class
        {
            return new IntPtr(addr).ToDelegate<T>();
        }

        public static Detour<T> DetourWith<T>(this long addr, T del) where T : class
        {
            return new IntPtr(addr).DetourWith(del);
        }

        #endregion
    }
}