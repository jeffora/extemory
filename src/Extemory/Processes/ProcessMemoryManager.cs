using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Extemory.Win32;
using Extemory.Injection;

namespace Extemory.Processes
{
    internal class ProcessMemoryManager : IDisposable
    {
        private const ProcessAccessFlags DefaultAccessFlags = 
            ProcessAccessFlags.QueryInformation | ProcessAccessFlags.CreateThread | 
            ProcessAccessFlags.VMOperation | ProcessAccessFlags.VMWrite | 
            ProcessAccessFlags.VMRead;

        private IntPtr _handle;
        private int _mainThreadId;
        private readonly Process _process;
        private readonly Dictionary<string, InjectedModule> _injectedModules;

        private ProcessMemoryManager(Process process)
        {
            _process = process;
            _injectedModules = new Dictionary<string, InjectedModule>();
        }

        public IntPtr Handle
        {
            get
            {
                if (_handle == IntPtr.Zero)
                {
                    _handle = Imports.OpenProcess(DefaultAccessFlags, false, _process.Id);
                    if (_handle == IntPtr.Zero)
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                return _handle;
            }
        }

        public int MainThreadId { get; set; }

        public Dictionary<string, InjectedModule> InjectedModules
        {
            get
            {
                return _injectedModules;
            }
        }

        private void CloseHandle()
        {
            if (_handle != IntPtr.Zero)
            {
                Imports.CloseHandle(_handle);
                _handle = IntPtr.Zero;
            }
        }

        #region IDisposable

        public void Dispose()
        {
            CloseHandle();
        }

        ~ProcessMemoryManager()
        {
            Dispose();
        }

        #endregion

        #region Static

        private static readonly Dictionary<int, ProcessMemoryManager> ProcessManagers = new Dictionary<int, ProcessMemoryManager>();
        private static bool _debugMode;

        internal static ProcessMemoryManager ForProcess(Process process)
        {
            // Make sure we have the SeDebugPrivelege token
            //if (!_debugMode)
            //{
            //    Process.EnterDebugMode();
            //    _debugMode = true;
            //}

            if (!ProcessManagers.ContainsKey(process.Id))
            {
                ProcessManagers[process.Id] = new ProcessMemoryManager(process);
            }
            return ProcessManagers[process.Id];
        }

        #endregion
    }
}
