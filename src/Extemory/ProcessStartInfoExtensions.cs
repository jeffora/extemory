using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Extemory.Processes;
using Extemory.Win32;

namespace Extemory
{
    public static class ProcessStartInfoExtensions
    {
        public static Process CreateProcessWithFlags(this ProcessStartInfo startInfo, ProcessCreationFlags creationFlags)
        {
            if (string.IsNullOrEmpty(startInfo.FileName))
                throw new ArgumentException("No FileName was specified in ProcessStartInfo", "startInfo");
            if (!File.Exists(startInfo.FileName))
                throw new FileNotFoundException("Unable to find the specified the process file", startInfo.FileName);

            var startupInfo = new STARTUPINFO();
            startupInfo.cb = Marshal.SizeOf(startupInfo);

            var args = string.IsNullOrEmpty(startInfo.Arguments) ? null : new StringBuilder(startInfo.Arguments);
            var workingDirectory = string.IsNullOrEmpty(startInfo.WorkingDirectory) ? null : startInfo.WorkingDirectory;

            var procInfo = new PROCESS_INFORMATION();

            if (!Imports.CreateProcess(startInfo.FileName, args, IntPtr.Zero, IntPtr.Zero, false, creationFlags, IntPtr.Zero, workingDirectory, ref startupInfo, out procInfo))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            var ret = Process.GetProcessById(procInfo.dwProcessId);
            ProcessMemoryManager.ForProcess(ret).MainThreadId = procInfo.dwThreadId;
            return ret;
        }

        public static Process CreateProcessSuspended(this ProcessStartInfo startInfo)
        {
            return startInfo.CreateProcessWithFlags(ProcessCreationFlags.CreateSuspended);
        }
    }
}