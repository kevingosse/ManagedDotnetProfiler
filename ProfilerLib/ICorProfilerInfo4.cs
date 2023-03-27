namespace ProfilerLib;

public class ICorProfilerInfo4 : ICorProfilerInfo3
{
    private NativeObjects.ICorProfilerInfo4Invoker _impl;

    public ICorProfilerInfo4(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult EnumThreads(out nint ppEnum)
    {
        return _impl.EnumThreads(out ppEnum);
    }

    public HResult InitializeCurrentThread()
    {
        return _impl.InitializeCurrentThread();
    }

    public unsafe HResult RequestReJIT(uint cFunctions, ModuleId* moduleIds, MdMethodDef* methodIds)
    {
        return _impl.RequestReJIT(cFunctions, moduleIds, methodIds);
    }

    public unsafe HResult RequestRevert(uint cFunctions, ModuleId* moduleIds, MdMethodDef* methodIds, HResult* status)
    {
        return _impl.RequestRevert(cFunctions, moduleIds, methodIds, status);
    }

    public unsafe HResult GetCodeInfo3(FunctionId functionID, ReJITId reJitId, uint cCodeInfos, out uint pcCodeInfos, COR_PRF_CODE_INFO* codeInfos)
    {
        return _impl.GetCodeInfo3(functionID, reJitId, cCodeInfos, out pcCodeInfos, codeInfos);
    }

    public unsafe HResult GetFunctionFromIP2(byte* ip, out FunctionId pFunctionId, out ReJITId pReJitId)
    {
        return _impl.GetFunctionFromIP2(ip, out pFunctionId, out pReJitId);
    }

    public unsafe HResult GetReJITIDs(FunctionId functionId, uint cReJitIds, uint* pcReJitIds, ReJITId* reJitIds)
    {
        return _impl.GetReJITIDs(functionId, cReJitIds, pcReJitIds, reJitIds);
    }

    public unsafe HResult GetILToNativeMapping2(FunctionId functionId, ReJITId reJitId, uint cMap, uint* pcMap, COR_DEBUG_IL_TO_NATIVE_MAP* map)
    {
        return _impl.GetILToNativeMapping2(functionId, reJitId, cMap, pcMap, map);
    }

    public HResult EnumJITedFunctions2(out nint ppEnum)
    {
        return _impl.EnumJITedFunctions2(out ppEnum);
    }

    public HResult GetObjectSize2(ObjectId objectId, out nint pcSize)
    {
        return _impl.GetObjectSize2(objectId, out pcSize);
    }
}