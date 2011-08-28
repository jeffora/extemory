using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Extemory.CustomMarshalling;
using Extemory.Processes;
using Extemory.Win32;

namespace Extemory.Injection
{
    public class InjectedModule
    {
        private const uint DefaultTimeout = (uint)ThreadWaitValue.Infinite;

        private readonly Dictionary<string, IntPtr> _exports;
        private readonly Process _parentProcess;

        internal InjectedModule(IntPtr baseAddress, string moduelName, Process process)
        {
            ModuleName = moduelName;
            BaseAddress = baseAddress;
            _parentProcess = process;
            _exports = new Dictionary<string, IntPtr>();
        }

        public string ModuleName { get; private set; }

        public IntPtr BaseAddress { get; private set; }

        public IntPtr CallExport(string funcName)
        {
            return CallExportInternal(DefaultTimeout, funcName, IntPtr.Zero, null, 0);
        }

        public IntPtr CallExport<T>(string funcName, T data) where T : struct
        {
            var pData = IntPtr.Zero;
            try
            {
                var dataSize = CustomMarshal.SizeOf(data);
                pData = Marshal.AllocHGlobal(dataSize);
                CustomMarshal.StructureToPtr(data, pData, true);
                return CallExportInternal(DefaultTimeout, funcName, pData, typeof(T), dataSize);
            }
            finally
            {
                if (pData != IntPtr.Zero)
                    Marshal.FreeHGlobal(pData);
            }
            
        }

        private IntPtr CallExportInternal(uint timeout, string funcName, IntPtr data, Type dataType, int dataSize)
        {
            var manager = ProcessMemoryManager.ForProcess(_parentProcess);
            if (!manager.InjectedModules.ContainsKey(ModuleName))
                throw new Exception("The injected module does not appear to be associated to its parent process. This could indicate corrupted process state");

            var pFunc = FindExport(funcName);

            var pDataRemote = IntPtr.Zero;
            var hThread = IntPtr.Zero;
            try
            {
                // check if we have all required parameters to pass a data parameter
                // if we don't, assume we aren't passing any data
                if (!(data == IntPtr.Zero || dataSize == 0 || dataType == null))
                {
                    // allocate memory in remote process for parameter
                    pDataRemote = Imports.VirtualAllocEx(_parentProcess.GetHandle(), IntPtr.Zero, (uint)dataSize, AllocationType.Commit, MemoryProtection.ReadWrite);
                    if (pDataRemote == IntPtr.Zero)
                        throw new Win32Exception(Marshal.GetLastWin32Error());

                    // rebase the data so that pointers point to valid memory locations for the target process
                    // this renders the unmanaged structure useless in this process - should be able to re-rebase back to
                    // this target process by calling CustomMarshal.RebaseUnmanagedStructure(data, data, dataType); but not tested
                    CustomMarshal.RebaseUnmanagedStructure(data, pDataRemote, dataType);

                    int bytesWritten;
                    if (!Imports.WriteProcessMemory(_parentProcess.GetHandle(), pDataRemote, data, (uint)dataSize, out bytesWritten))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                hThread = Imports.CreateRemoteThread(_parentProcess.GetHandle(), IntPtr.Zero, 0, pFunc, pDataRemote, 0, IntPtr.Zero);
                if (hThread == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var singleObject = Imports.WaitForSingleObject(hThread, timeout);
                if (!(singleObject == (uint)ThreadWaitValue.Object0 || singleObject == (uint)ThreadWaitValue.Timeout))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                IntPtr pRet;
                if (!Imports.GetExitCodeThread(hThread, out pRet))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                return pRet;
            }
            finally
            {
                if (pDataRemote != IntPtr.Zero)
                    Imports.VirtualFreeEx(_parentProcess.GetHandle(), pDataRemote, 0, AllocationType.Release);
                if (hThread != IntPtr.Zero)
                    Imports.CloseHandle(hThread);
            }
        }

        public void Eject()
        {
            var manager = ProcessMemoryManager.ForProcess(_parentProcess);
            if (!manager.InjectedModules.ContainsKey(ModuleName))
                throw new Exception("The injected module does not appear to be associated to its parent process. This could indicate corrupted process state");

            var hThread = IntPtr.Zero;
            try
            {
                var hKernel32 = Imports.GetModuleHandle("Kernel32");
                if (hKernel32 == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                var hFreeLib = Imports.GetProcAddress(hKernel32, "FreeLibrary");
                if (hFreeLib == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                hThread = Imports.CreateRemoteThread(_parentProcess.GetHandle(), IntPtr.Zero, 0, hFreeLib, BaseAddress, 0, IntPtr.Zero);
                if (hThread == IntPtr.Zero)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                if (Imports.WaitForSingleObject(hThread, (uint)ThreadWaitValue.Infinite) != (uint)ThreadWaitValue.Object0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                IntPtr pFreeLibRet;
                if (!Imports.GetExitCodeThread(hThread, out pFreeLibRet))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                if (pFreeLibRet == IntPtr.Zero)
                    throw new Exception("FreeLibrary failed in remote process");
            }
            finally
            {
                if (hThread != IntPtr.Zero)
                    Imports.CloseHandle(hThread);
            }
        }

        private IntPtr FindExport(string functionName)
        {
            if (_exports.ContainsKey(functionName))
            {
                return _exports[functionName];
            }

            var hModule = IntPtr.Zero;
            try
            {
                hModule = Imports.LoadLibraryEx(ModuleName, IntPtr.Zero, LoadLibraryExFlags.DontResolveDllReferences);
                if (hModule == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                var pFunc = Imports.GetProcAddress(hModule, functionName);
                if (pFunc == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                // Get RVA of export and add to base address of injected module
                // hack at the moment to deal with x64
                var addr = IntPtr.Size == 8
                               ? new IntPtr(BaseAddress.ToInt64() + (pFunc.ToInt64() - hModule.ToInt64()))
                               : new IntPtr(BaseAddress.ToInt32() + (pFunc.ToInt32() - hModule.ToInt32()));
                _exports[functionName] = addr;
                return addr;
            }
            finally
            {
                if (hModule != IntPtr.Zero)
                {
                    Imports.FreeLibrary(hModule);
                }
            }
        }
    }
}