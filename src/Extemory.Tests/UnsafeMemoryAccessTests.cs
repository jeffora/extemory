using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Extemory.Win32;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Extemory.Tests
{
    [TestClass]
    public class UnsafeMemoryAccessTests
    {
        [TestMethod]
        public void TestPointerCanWriteToMarshalMemory()
        {
            var pMem = IntPtr.Zero;
            try
            {
                pMem = Marshal.AllocHGlobal(4);
                var test = 0xDEADBEEF;
                unsafe
                {
                    var pData = (uint*) pMem.ToPointer();
                    *pData = test;
                }
                var assert = (uint)Marshal.ReadInt32(pMem);
                Assert.AreEqual(assert, test);
            }
            finally
            {
                if (pMem != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pMem);
                }
            }
        }

        [TestMethod]
        public void TestPointerCanWriteToVirtualAllocMemory()
        {
            var hProcess = Imports.OpenProcess(ProcessAccessFlags.VMOperation /* required for VirtualAllocEx */, false,
                                               Process.GetCurrentProcess().Id);
            if (hProcess == IntPtr.Zero)
                throw new Exception("Unable to open process");

            var pMem = IntPtr.Zero;
            try
            {
                pMem = Imports.VirtualAllocEx(hProcess, IntPtr.Zero, 4, AllocationType.Commit,
                                              MemoryProtection.ReadWrite);
                if (pMem == IntPtr.Zero)
                    throw new Exception("Unable to allocate memory");
                var test = 0xDEADBEEF;
                unsafe
                {
                    var pData = (uint*) pMem.ToPointer();
                    *pData = test;
                }
                var assert = (uint)Marshal.ReadInt32(pMem);
                Assert.AreEqual(assert, test);
            }
            finally
            {
                if (pMem != IntPtr.Zero)
                {
                }
                if (hProcess != IntPtr.Zero)
                {
                    Imports.CloseHandle(hProcess);
                }
            }
        }
    }
}
