namespace ProfilerLib;

public class ICorProfilerInfo8 : ICorProfilerInfo7
{
    private NativeObjects.ICorProfilerInfo8Invoker _impl;

    public ICorProfilerInfo8(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult IsFunctionDynamic(FunctionId functionId, out int isDynamic)
    {
        return _impl.IsFunctionDynamic(functionId, out isDynamic);
    }

    public unsafe HResult GetFunctionFromIP3(nint ip, FunctionId* functionId, out ReJITId pReJitId)
    {
        return _impl.GetFunctionFromIP3(ip, functionId, out pReJitId);
    }

    public unsafe HResult GetDynamicFunctionInfo(FunctionId functionId, out ModuleId moduleId, byte* ppvSig, out uint pbSig, uint cchName, out uint pcchName, char* wszName)
    {
        return _impl.GetDynamicFunctionInfo(functionId, out moduleId, ppvSig, out pbSig, cchName, out pcchName, wszName);
    }
}