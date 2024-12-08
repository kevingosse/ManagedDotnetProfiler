namespace ProfilerLib;

public class ICorProfilerInfo11 : ICorProfilerInfo10
{
    private NativeObjects.ICorProfilerInfo11Invoker _impl;

    public ICorProfilerInfo11(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public unsafe HResult GetEnvironmentVariable(string name, Span<char> value, out uint valueLength)
    {
        fixed (char* pValue = value)
        fixed (char* pName = name)
        {
            return _impl.GetEnvironmentVariable(pName, (uint)value.Length, out valueLength, pValue);
        }
    }

    public unsafe HResult<string> GetEnvironmentVariable(string name)
    {
        var result = GetEnvironmentVariable(name, Span<char>.Empty, out var length);

        if (!result)
        {
            return result;
        }

        Span<char> buffer = stackalloc char[(int)length];

        result = GetEnvironmentVariable(name, buffer, out _);

        if (!result)
        {
            return result;
        }

        return new(result, buffer.WithoutNullTerminator());
    }

    public unsafe HResult SetEnvironmentVariable(string name, string value)
    {
        fixed (char* pName = name)
        fixed (char* pValue = value)
        {
            return _impl.SetEnvironmentVariable(pName, pValue);
        }
    }
}