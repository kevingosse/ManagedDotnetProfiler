namespace ProfilerLib;

public class ICorProfilerInfo3 : ICorProfilerInfo2
{
    private NativeObjects.ICorProfilerInfo3Invoker _impl;

    public ICorProfilerInfo3(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public unsafe HResult EnumJITedFunctions(out void* ppEnum)
    {
        return _impl.EnumJITedFunctions(out ppEnum);
    }

    public HResult RequestProfilerDetach(int dwExpectedCompletionMilliseconds)
    {
        return _impl.RequestProfilerDetach(dwExpectedCompletionMilliseconds);
    }

    public unsafe HResult SetFunctionIDMapper2(delegate*unmanaged[Stdcall]<FunctionId, void*, bool*, nint> pFunc, void* clientData)
    {
        return _impl.SetFunctionIDMapper2(pFunc, clientData);
    }

    public HResult GetStringLayout2(out uint pStringLengthOffset, out uint pBufferOffset)
    {
        return _impl.GetStringLayout2(out pStringLengthOffset, out pBufferOffset);
    }

    public unsafe HResult SetEnterLeaveFunctionHooks3(void* pFuncEnter3, void* pFuncLeave3, void* pFuncTailcall3)
    {
        return _impl.SetEnterLeaveFunctionHooks3(pFuncEnter3, pFuncLeave3, pFuncTailcall3);
    }

    public unsafe HResult SetEnterLeaveFunctionHooks3WithInfo(void* pFuncEnter3WithInfo, void* pFuncLeave3WithInfo, void* pFuncTailcall3WithInfo)
    {
        return _impl.SetEnterLeaveFunctionHooks3WithInfo(pFuncEnter3WithInfo, pFuncLeave3WithInfo, pFuncTailcall3WithInfo);
    }

    public unsafe HResult GetFunctionEnter3Info(FunctionId functionId, COR_PRF_ELT_INFO eltInfo, out COR_PRF_FRAME_INFO pFrameInfo, int* pcbArgumentInfo, COR_PRF_FUNCTION_ARGUMENT_INFO* pArgumentInfo)
    {
        return _impl.GetFunctionEnter3Info(functionId, eltInfo, out pFrameInfo, pcbArgumentInfo, pArgumentInfo);
    }

    public HResult GetFunctionLeave3Info(FunctionId functionId, COR_PRF_ELT_INFO eltInfo, out COR_PRF_FRAME_INFO pFrameInfo, out COR_PRF_FUNCTION_ARGUMENT_RANGE pRetvalRange)
    {
        return _impl.GetFunctionLeave3Info(functionId, eltInfo, out pFrameInfo, out pRetvalRange);
    }

    public HResult GetFunctionTailcall3Info(FunctionId functionId, COR_PRF_ELT_INFO eltInfo, out COR_PRF_FRAME_INFO pFrameInfo)
    {
        return _impl.GetFunctionTailcall3Info(functionId, eltInfo, out pFrameInfo);
    }

    public unsafe HResult EnumModules(out void* ppEnum)
    {
        return _impl.EnumModules(out ppEnum);
    }

    public unsafe HResult GetRuntimeInformation(out ushort pClrInstanceId, out COR_PRF_RUNTIME_TYPE pRuntimeType, out ushort pMajorVersion, out ushort pMinorVersion, out ushort pBuildNumber, out ushort pQFEVersion, uint cchVersionString, out uint pcchVersionString, char* szVersionString)
    {
        return _impl.GetRuntimeInformation(out pClrInstanceId, out pRuntimeType, out pMajorVersion, out pMinorVersion, out pBuildNumber, out pQFEVersion, cchVersionString, out pcchVersionString, szVersionString);
    }

    public unsafe HResult GetThreadStaticAddress2(ClassId classId, MdFieldDef fieldToken, AppDomainId appDomainId, ThreadId threadId, out void* ppAddress)
    {
        return _impl.GetThreadStaticAddress2(classId, fieldToken, appDomainId, threadId, out ppAddress);
    }

    public unsafe HResult GetAppDomainsContainingModule(ModuleId moduleId, uint cAppDomainIds, out uint pcAppDomainIds, AppDomainId* appDomainIds)
    {
        return _impl.GetAppDomainsContainingModule(moduleId, cAppDomainIds, out pcAppDomainIds, appDomainIds);
    }

    public unsafe HResult GetModuleInfo2(ModuleId moduleId, out byte* ppBaseLoadAddress, uint cchName, out uint pcchName, char* szName, out AssemblyId pAssemblyId, out int pdwModuleFlags)
    {
        return _impl.GetModuleInfo2(moduleId, out ppBaseLoadAddress, cchName, out pcchName, szName, out pAssemblyId, out pdwModuleFlags);
    }
}