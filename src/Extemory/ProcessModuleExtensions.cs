using System;
using System.Diagnostics;

namespace Extemory
{
    public static class ProcessModuleExtensions
    {
        #region Injection

        public static void CallExport(this ProcessModule module, Process targetProc, string exportName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}