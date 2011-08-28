using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extemory.Tests
{
    [TestClass]
    public class ProcessExtensionsTests
    {
        #region Read Primitive types

        [TestMethod]
        public void TestReadPrimitiveTypeShort()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (short)rand.Next(short.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteInt16(pAddr, value);
            try
            {
                var test = proc.Read<short>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeUShort()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (ushort)rand.Next(ushort.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteInt16(pAddr, (short)value);
            try
            {
                var test = proc.Read<ushort>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeInt()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = rand.Next();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteInt32(pAddr, value);
            try
            {
                var test = proc.Read<int>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeUInt()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (uint)rand.Next();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteInt32(pAddr, (int)value);
            try
            {
                var test = proc.Read<uint>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeLong()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (long)rand.NextDouble() * long.MaxValue;

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteInt64(pAddr, value);
            try
            {
                var test = proc.Read<long>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeULong()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (ulong)rand.NextDouble() * ulong.MaxValue;

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteInt64(pAddr, (long)value);
            try
            {
                var test = proc.Read<ulong>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeIntPtr()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = new IntPtr(rand.Next());

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteIntPtr(pAddr, value);
            try
            {
                var test = proc.Read<IntPtr>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeBoolean()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = rand.Next() > (int.MaxValue / 2);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            var valBytes = BitConverter.GetBytes(value);
            for (var i = 0; i < valBytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, valBytes[i]);
            }
            try
            {
                var test = proc.Read<bool>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeByte()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (byte)rand.Next(byte.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteByte(pAddr, value);
            try
            {
                var test = proc.Read<byte>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeSByte()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (sbyte)rand.Next(sbyte.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            Marshal.WriteByte(pAddr, (byte)value);
            try
            {
                var test = proc.Read<sbyte>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeChar()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (char)rand.Next(char.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SystemDefaultCharSize);
            var valBytes = BitConverter.GetBytes(value);
            for (var i = 0; i < valBytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, valBytes[i]);
            }
            try
            {
                var test = proc.Read<char>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeSingle()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (float)rand.NextDouble();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            var valBytes = BitConverter.GetBytes(value);
            for (var i = 0; i < valBytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, valBytes[i]);
            }
            try
            {
                var test = proc.Read<float>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeDouble()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = rand.NextDouble();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            var valBytes = BitConverter.GetBytes(value);
            for (var i = 0; i < valBytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, valBytes[i]);
            }
            try
            {
                var test = proc.Read<double>(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        #endregion

        #region Write Primitive types

        [TestMethod]
        public void TestWritePrimitiveTypeShort()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (short)rand.Next(short.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = Marshal.ReadInt16(pAddr);
                Assert.AreEqual(value, test);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeUShort()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (ushort)rand.Next(ushort.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = (ushort)Marshal.ReadInt16(pAddr);
                Assert.AreEqual(value, test);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeInt()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = rand.Next();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = Marshal.ReadInt32(pAddr);
                Assert.AreEqual(value, test);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeUInt()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (uint)rand.Next();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = (uint)Marshal.ReadInt32(pAddr);
                Assert.AreEqual(value, test);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeLong()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (long)rand.NextDouble() * long.MaxValue;

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = Marshal.ReadInt64(pAddr);
                Assert.AreEqual(value, test);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeULong()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (ulong)rand.NextDouble() * ulong.MaxValue;

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = (ulong)Marshal.ReadInt64(pAddr);
                Assert.AreEqual(value, test);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeIntPtr()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = new IntPtr(rand.Next());

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = Marshal.ReadIntPtr(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeBoolean()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = rand.Next() > (int.MaxValue / 2);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var valBytes = new byte[Marshal.SizeOf(value)];
                for (var i = 0; i < valBytes.Length; i++)
                {
                    valBytes[i] = Marshal.ReadByte(pAddr, i);
                }
                var test = BitConverter.ToBoolean(valBytes, 0);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeByte()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (byte)rand.Next(byte.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = Marshal.ReadByte(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeSByte()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (sbyte)rand.Next(sbyte.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var test = (sbyte)Marshal.ReadByte(pAddr);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeChar()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (char)rand.Next(char.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SystemDefaultCharSize);
            try
            {
                proc.Write(pAddr, value);
                var valBytes = new byte[Marshal.SystemDefaultCharSize];
                for (int i = 0; i < valBytes.Length; i++)
                {
                    valBytes[i] = Marshal.ReadByte(pAddr, i);
                }
                var test = BitConverter.ToChar(valBytes, 0);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeSingle()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (float)rand.NextDouble();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var valBytes = new byte[Marshal.SizeOf(value)];
                for (int i = 0; i < valBytes.Length; i++)
                {
                    valBytes[i] = Marshal.ReadByte(pAddr, i);
                }
                var test = BitConverter.ToSingle(valBytes, 0);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeDouble()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = rand.NextDouble();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                var valBytes = new byte[Marshal.SizeOf(value)];
                for (int i = 0; i < valBytes.Length; i++)
                {
                    valBytes[i] = Marshal.ReadByte(pAddr, i);
                }
                var test = BitConverter.ToDouble(valBytes, 0);
                Assert.AreEqual(test, value);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        #endregion

        #region Read Primitive array types

        [TestMethod]
        public void TestReadPrimitiveTypeIntArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new int[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = rand.Next();
                Marshal.WriteInt32(pAddr, i * 4, values[i]);
            }
            try
            {
                var test = proc.ReadArray<int>(pAddr, values.Length);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeShortArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new short[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = (short)rand.Next(short.MaxValue);
                Marshal.WriteInt16(pAddr, i * 2, values[i]);
            }
            try
            {
                var test = proc.ReadArray<short>(pAddr, values.Length);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeLongArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new long[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(long)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = (long)(rand.NextDouble() * long.MaxValue);
                Marshal.WriteInt64(pAddr, i * 8, values[i]);
            }
            try
            {
                var test = proc.ReadArray<long>(pAddr, values.Length);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadPrimitiveTypeDoubleArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new double[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = rand.NextDouble();
                var bytes = BitConverter.GetBytes(values[i]);
                for (int j = 0; j < bytes.Length; j++)
                {
                    Marshal.WriteByte(pAddr, (i * 8) + j, bytes[j]);
                }
            }
            try
            {
                var test = proc.ReadArray<double>(pAddr, values.Length);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadPrimitiveTypeUIntArrayFailsInvalid()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new uint[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = (uint)rand.Next();
                Marshal.WriteInt32(pAddr, i * 4, (int)values[i]);
            }
            try
            {
                var test = proc.ReadArray<uint>(pAddr, values.Length);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        #endregion

        #region Write Primitive array types

        [TestMethod]
        public void TestWritePrimitiveTypeIntArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new int[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = rand.Next();
            }
            try
            {
                proc.WriteArray(pAddr, values);
                var test = new int[arrSize];
                Marshal.Copy(pAddr, test, 0, arrSize);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeShortArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new short[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(short)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = (short)rand.Next(short.MaxValue);
            }
            try
            {
                proc.WriteArray(pAddr, values);
                var test = new short[arrSize];
                Marshal.Copy(pAddr, test, 0, arrSize);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeLongArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new long[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(long)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = (long)(rand.NextDouble() * long.MaxValue);
            }
            try
            {
                proc.WriteArray(pAddr, values);
                var test = new long[arrSize];
                Marshal.Copy(pAddr, test, 0, arrSize);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWritePrimitiveTypeDoubleArray()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new double[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(double)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = rand.NextDouble();
            }
            try
            {
                proc.WriteArray(pAddr, values);
                var test = new double[arrSize];
                Marshal.Copy(pAddr, test, 0, arrSize);
                CollectionAssert.AreEqual(values.ToList(), test.ToList());
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestWritePrimitiveTypeUIntArrayFailsInvalid()
        {
            var rand = new Random();
            var arrSize = rand.Next(1, 10);
            var values = new uint[arrSize];
            var proc = Process.GetCurrentProcess();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)) * arrSize);
            for (int i = 0; i < arrSize; i++)
            {
                values[i] = (uint)rand.Next();
            }
            try
            {
                proc.WriteArray(pAddr, values);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        #endregion

        #region Read String

        [TestMethod]
        public void TestReadAsciiString()
        {
            var encoding = Encoding.ASCII;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            var expected = encoding.GetString(bytes);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, bytes[i]);
            }
            try
            {
                var result = proc.ReadString(pAddr, encoding);
                Assert.AreEqual(expected, result);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadUnicodeString()
        {
            var encoding = Encoding.Unicode;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            var expected = encoding.GetString(bytes);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, bytes[i]);
            }
            try
            {
                var result = proc.ReadString(pAddr, encoding);
                Assert.AreEqual(expected, result);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestReadUtf8String()
        {
            var encoding = Encoding.UTF8;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            var expected = encoding.GetString(bytes);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, bytes[i]);
            }
            try
            {
                var result = proc.ReadString(pAddr, encoding);
                Assert.AreEqual(expected, result);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadUtf7String()
        {
            var encoding = Encoding.UTF7;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            var expected = encoding.GetString(bytes);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, bytes[i]);
            }
            try
            {
                var result = proc.ReadString(pAddr, encoding);
                Assert.AreEqual(expected, result);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReadUtf32String()
        {
            var encoding = Encoding.UTF32;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            var expected = encoding.GetString(bytes);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
            {
                Marshal.WriteByte(pAddr, i, bytes[i]);
            }
            try
            {
                var result = proc.ReadString(pAddr, encoding);
                Assert.AreEqual(expected, result);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        #endregion

        #region Write String

        [TestMethod]
        public void TestWriteAsciiString()
        {
            var encoding = Encoding.ASCII;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var expected = encoding.GetString(bytes);
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                proc.WriteString(pAddr, testString, encoding);
                var resultBuffer = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultBuffer[i] = Marshal.ReadByte(pAddr, i);
                }
                var result = encoding.GetString(resultBuffer);
                Assert.AreEqual(result, expected);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWriteUnicodeString()
        {
            var encoding = Encoding.Unicode;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var expected = encoding.GetString(bytes);
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                proc.WriteString(pAddr, testString, encoding);
                var resultBuffer = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultBuffer[i] = Marshal.ReadByte(pAddr, i);
                }
                var result = encoding.GetString(resultBuffer);
                Assert.AreEqual(result, expected);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWriteUtf8String()
        {
            var encoding = Encoding.UTF8;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var expected = encoding.GetString(bytes);
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                proc.WriteString(pAddr, testString, encoding);
                var resultBuffer = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultBuffer[i] = Marshal.ReadByte(pAddr, i);
                }
                var result = encoding.GetString(resultBuffer);
                Assert.AreEqual(result, expected);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWriteUtf7String()
        {
            var encoding = Encoding.UTF7;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var expected = encoding.GetString(bytes);
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                proc.WriteString(pAddr, testString, encoding);
                var resultBuffer = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultBuffer[i] = Marshal.ReadByte(pAddr, i);
                }
                var result = encoding.GetString(resultBuffer);
                Assert.AreEqual(result, expected);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestWriteUtf32String()
        {
            var encoding = Encoding.UTF32;
            var proc = Process.GetCurrentProcess();
            var testString = "Thîs îs ùnìcôdé";

            var bytes = encoding.GetBytes(testString);
            bytes = bytes.Concat(encoding.GetBytes(new[] { '\0' })).ToArray();
            var expected = encoding.GetString(bytes);
            var pAddr = Marshal.AllocHGlobal(bytes.Length);
            try
            {
                proc.WriteString(pAddr, testString, encoding);
                var resultBuffer = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultBuffer[i] = Marshal.ReadByte(pAddr, i);
                }
                var result = encoding.GetString(resultBuffer);
                Assert.AreEqual(result, expected);
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        #endregion

        #region Round trip

        [TestMethod]
        public void TestEnsureRoundTripShort()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (short)rand.Next(short.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                Assert.AreEqual(value, proc.Read<short>(pAddr));
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestEnsureRoundTripInt()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = rand.Next();

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                Assert.AreEqual(value, proc.Read<int>(pAddr));
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestEnsureRoundTripLong()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (long)rand.NextDouble() * long.MaxValue;

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SizeOf(value));
            try
            {
                proc.Write(pAddr, value);
                Assert.AreEqual(value, proc.Read<long>(pAddr));
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestEnsureRoundTripChar()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = (char)rand.Next(char.MaxValue);

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SystemDefaultCharSize);
            try
            {
                proc.Write(pAddr, value);
                Assert.AreEqual(value, proc.Read<char>(pAddr));
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        [TestMethod]
        public void TestEnsureRoundTripIntPtr()
        {
            var rand = new Random();
            var proc = Process.GetCurrentProcess();
            var value = new IntPtr(rand.Next());

            IntPtr pAddr = Marshal.AllocHGlobal(Marshal.SystemDefaultCharSize);
            try
            {
                proc.Write(pAddr, value);
                Assert.AreEqual(value, proc.Read<IntPtr>(pAddr));
            }
            finally
            {
                if (pAddr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pAddr);
                }
            }
        }

        #endregion
    }
}
