namespace ProfilerLib;

public class ICorProfilerInfo11 : ICorProfilerInfo10
{
    private NativeObjects.ICorProfilerInfo11Invoker _impl;

    public ICorProfilerInfo11(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public unsafe HResult GetEnvironmentVariable(char* szName, uint cchValue, out uint pcchValue, char* szValue)
    {
        return _impl.GetEnvironmentVariable(szName, cchValue, out pcchValue, szValue);
    }

    public unsafe HResult SetEnvironmentVariable(char* szName, char* szValue)
    {
        return _impl.SetEnvironmentVariable(szName, szValue);
    }
}