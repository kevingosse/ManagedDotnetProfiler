namespace ProfilerLib;

public class ICorProfilerInfo8 : ICorProfilerInfo7
{
    private NativeObjects.ICorProfilerInfo8Invoker _impl;

    public ICorProfilerInfo8(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult<bool> IsFunctionDynamic(FunctionId functionId)
    {
        var result = _impl.IsFunctionDynamic(functionId, out var isDynamic);
        return new(result, isDynamic != 0);
    }

    public unsafe HResult<FunctionFromIP> GetFunctionFromIP3(nint ip)
    {
        var result = _impl.GetFunctionFromIP3(ip, out var functionId, out var reJitId);
        return new(result, new(functionId, reJitId));
    }

    public unsafe HResult<DynamicFunctionInfo> GetDynamicFunctionInfo(FunctionId functionId, Span<char> name, out uint nameLength)
    {
        fixed (char* pName = name)
        {
            var result = _impl.GetDynamicFunctionInfo(functionId, out var moduleId, out var sig, out var pbSig, (uint)name.Length, out nameLength, pName);
            return new(result, new(moduleId, sig, pbSig));
        }
    }

    public unsafe HResult<DynamicFunctionInfoWithName> GetDynamicFunctionInfo(FunctionId functionId)
    {
        var (result, _) = GetDynamicFunctionInfo(functionId, Span<char>.Empty, out var length);

        if (!result)
        {
            return result;
        }

        Span<char> buffer = stackalloc char[(int)length];

        (result, var functionInfo) = GetDynamicFunctionInfo(functionId, buffer, out _);

        if (!result)
        {
            return result;
        }

        return new(result, new(functionInfo.ModuleId, functionInfo.SignaturePtr, functionInfo.SignatureLength, buffer.ToString()));
    }
}