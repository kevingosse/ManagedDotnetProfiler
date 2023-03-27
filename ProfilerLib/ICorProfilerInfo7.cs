namespace ProfilerLib;

public class ICorProfilerInfo7 : ICorProfilerInfo6
{
    private NativeObjects.ICorProfilerInfo7Invoker _impl;

    public ICorProfilerInfo7(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult ApplyMetaData(ModuleId moduleId)
    {
        return _impl.ApplyMetaData(moduleId);
    }

    public HResult GetInMemorySymbolsLength(ModuleId moduleId, out int pCountSymbolBytes)
    {
        return _impl.GetInMemorySymbolsLength(moduleId, out pCountSymbolBytes);
    }

    public unsafe HResult ReadInMemorySymbols(ModuleId moduleId, int symbolsReadOffset, byte* pSymbolBytes, int countSymbolBytes, out int pCountSymbolBytesRead)
    {
        return _impl.ReadInMemorySymbols(moduleId, symbolsReadOffset, pSymbolBytes, countSymbolBytes, out pCountSymbolBytesRead);
    }
}