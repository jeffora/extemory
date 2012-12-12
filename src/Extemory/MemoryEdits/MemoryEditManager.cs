using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Extemory.MemoryEdits
{
    public class MemoryEditManager : IDisposable
    {
        private readonly Dictionary<IntPtr, IMemoryEdit> _applicationEdits;

        private MemoryEditManager()
        {
            _applicationEdits = new Dictionary<IntPtr, IMemoryEdit>();
        }

        public IMemoryEdit this[IntPtr addr]
        {
            get
            {
                IMemoryEdit edit;
                return _applicationEdits.TryGetValue(addr, out edit) ? edit : null;
            }
            internal set
            {
                if (_applicationEdits.ContainsKey(addr))
                {
                    _applicationEdits[addr].Dispose();
                }
                _applicationEdits[addr] = value;
            }
        }

        public IEnumerable<IMemoryEdit> Edits
        {
            get { return _applicationEdits.Values; }
        }

        #region IDisposable

        public void Dispose()
        {
            foreach (var edit in Edits)
            {
                edit.Dispose();
            }
            _applicationEdits.Clear();
        }

        ~MemoryEditManager()
        {
            Dispose();
        }

        #endregion

        #region Static

        private static readonly Dictionary<int, MemoryEditManager> ProcessManagers = new Dictionary<int, MemoryEditManager>();

        public static MemoryEditManager ForProcess(Process process)
        {
            process.Exited += ProcessExited;
            if (!ProcessManagers.ContainsKey(process.Id))
            {
                ProcessManagers[process.Id] = new MemoryEditManager();
            }
            return ProcessManagers[process.Id];
        }

        private static void ProcessExited(object sender, EventArgs e)
        {
            var procSender = (Process)sender;
            procSender.Exited -= ProcessExited;
            var manager = ForProcess(procSender);
            manager.Dispose();
            ProcessManagers.Remove(procSender.Id);
        }

        #endregion
    }
}
