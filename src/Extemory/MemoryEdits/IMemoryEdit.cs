using System;

namespace Extemory.MemoryEdits
{
    public interface IMemoryEdit : IDisposable
    {
        bool IsApplied { get; }
        void Apply();
        void Remove();
    }
}