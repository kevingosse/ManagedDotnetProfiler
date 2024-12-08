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

    public HResult<uint> GetInMemorySymbolsLength(ModuleId moduleId)
    {
        var result = _impl.GetInMemorySymbolsLength(moduleId, out var countSymbolBytes);
        return new(result, countSymbolBytes);
    }

    public unsafe HResult<uint> ReadInMemorySymbols(ModuleId moduleId, int symbolsReadOffset, Span<byte> symbolBytes)
    {
        fixed (byte* pSymbolBytes = symbolBytes)
        {
            var result = _impl.ReadInMemorySymbols(moduleId, symbolsReadOffset, pSymbolBytes, (uint)symbolBytes.Length, out var countSymbolBytesRead);
            return new(result, countSymbolBytesRead);
        }
    }
}