namespace ProfilerLib;

public class ICorProfilerInfo2 : ICorProfilerInfo
{
    private NativeObjects.ICorProfilerInfo2Invoker _impl;

    public ICorProfilerInfo2(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public unsafe HResult DoStackSnapshot(ThreadId thread, delegate*unmanaged[Stdcall]<FunctionId, nint, COR_PRF_FRAME_INFO, uint, byte*, void*, HResult> callback, uint infoFlags, void* clientData, byte* context, uint contextSize)
    {
        return _impl.DoStackSnapshot(thread, callback, infoFlags, clientData, context, contextSize);
    }

    public unsafe HResult SetEnterLeaveFunctionHooks2(void* pFuncEnter, void* pFuncLeave, void* pFuncTailcall)
    {
        return _impl.SetEnterLeaveFunctionHooks2(pFuncEnter, pFuncLeave, pFuncTailcall);
    }

    public unsafe HResult GetFunctionInfo2(FunctionId funcId, COR_PRF_FRAME_INFO frameInfo, out ClassId pClassId, out ModuleId pModuleId, out MdToken pToken, uint cTypeArgs, out uint pcTypeArgs, out ClassId* typeArgs)
    {
        return _impl.GetFunctionInfo2(funcId, frameInfo, out pClassId, out pModuleId, out pToken, cTypeArgs, out pcTypeArgs, out typeArgs);
    }

    public HResult GetStringLayout(out uint pBufferLengthOffset, out uint pStringLengthOffset, out uint pBufferOffset)
    {
        return _impl.GetStringLayout(out pBufferLengthOffset, out pStringLengthOffset, out pBufferOffset);
    }

    public HResult<StringLayout> GetStringLayout()
    {
        var result = _impl.GetStringLayout(out var pBufferLengthOffset, out var pStringLengthOffset, out var pBufferOffset);
        return new(result, new(pBufferLengthOffset, pStringLengthOffset, pBufferOffset));
    }

    public unsafe HResult GetClassLayout(ClassId classID, out COR_FIELD_OFFSET* rFieldOffset, uint cFieldOffset, out uint pcFieldOffset, out uint pulClassSize)
    {
        return _impl.GetClassLayout(classID, out rFieldOffset, cFieldOffset, out pcFieldOffset, out pulClassSize);
    }

    public unsafe HResult GetClassIDInfo2(ClassId classId, out ModuleId pModuleId, out MdTypeDef pTypeDefToken, out ClassId pParentClassId, uint cNumTypeArgs, out uint pcNumTypeArgs, out ClassId* typeArgs)
    {
        return _impl.GetClassIDInfo2(classId, out pModuleId, out pTypeDefToken, out pParentClassId, cNumTypeArgs, out pcNumTypeArgs, out typeArgs);
    }

    public unsafe HResult GetCodeInfo2(FunctionId functionID, uint cCodeInfos, out uint pcCodeInfos, out COR_PRF_CODE_INFO* codeInfos)
    {
        return _impl.GetCodeInfo2(functionID, cCodeInfos, out pcCodeInfos, out codeInfos);
    }

    public unsafe HResult GetClassFromTokenAndTypeArgs(ModuleId moduleID, MdTypeDef typeDef, uint cTypeArgs, ClassId* typeArgs, out ClassId pClassID)
    {
        return _impl.GetClassFromTokenAndTypeArgs(moduleID, typeDef, cTypeArgs, typeArgs, out pClassID);
    }

    public unsafe HResult GetFunctionFromTokenAndTypeArgs(ModuleId moduleID, MdMethodDef funcDef, ClassId classId, uint cTypeArgs, ClassId* typeArgs, out FunctionId pFunctionID)
    {
        return _impl.GetFunctionFromTokenAndTypeArgs(moduleID, funcDef, classId, cTypeArgs, typeArgs, out pFunctionID);
    }

    public unsafe HResult EnumModuleFrozenObjects(ModuleId moduleID, out void* ppEnum)
    {
        return _impl.EnumModuleFrozenObjects(moduleID, out ppEnum);
    }

    public unsafe HResult GetArrayObjectInfo(ObjectId objectId, uint cDimensions, out uint* pDimensionSizes, out int* pDimensionLowerBounds, out byte* ppData)
    {
        return _impl.GetArrayObjectInfo(objectId, cDimensions, out pDimensionSizes, out pDimensionLowerBounds, out ppData);
    }

    public HResult GetBoxClassLayout(ClassId classId, out uint pBufferOffset)
    {
        return _impl.GetBoxClassLayout(classId, out pBufferOffset);
    }

    public HResult GetThreadAppDomain(ThreadId threadId, out AppDomainId pAppDomainId)
    {
        return _impl.GetThreadAppDomain(threadId, out pAppDomainId);
    }

    public unsafe HResult GetRVAStaticAddress(ClassId classId, MdFieldDef fieldToken, out void* ppAddress)
    {
        return _impl.GetRVAStaticAddress(classId, fieldToken, out ppAddress);
    }

    public unsafe HResult GetAppDomainStaticAddress(ClassId classId, MdFieldDef fieldToken, AppDomainId appDomainId, out void* ppAddress)
    {
        return _impl.GetAppDomainStaticAddress(classId, fieldToken, appDomainId, out ppAddress);
    }

    public unsafe HResult GetThreadStaticAddress(ClassId classId, MdFieldDef fieldToken, ThreadId threadId, out void* ppAddress)
    {
        return _impl.GetThreadStaticAddress(classId, fieldToken, threadId, out ppAddress);
    }

    public unsafe HResult GetContextStaticAddress(ClassId classId, MdFieldDef fieldToken, ContextId contextId, out void* ppAddress)
    {
        return _impl.GetContextStaticAddress(classId, fieldToken, contextId, out ppAddress);
    }

    public HResult GetStaticFieldInfo(ClassId classId, MdFieldDef fieldToken, out COR_PRF_STATIC_TYPE pFieldInfo)
    {
        return _impl.GetStaticFieldInfo(classId, fieldToken, out pFieldInfo);
    }

    public unsafe HResult GetGenerationBounds(uint cObjectRanges, out uint pcObjectRanges, out COR_PRF_GC_GENERATION_RANGE* ranges)
    {
        return _impl.GetGenerationBounds(cObjectRanges, out pcObjectRanges, out ranges);
    }

    public HResult GetObjectGeneration(ObjectId objectId, out COR_PRF_GC_GENERATION_RANGE range)
    {
        return _impl.GetObjectGeneration(objectId, out range);
    }

    public HResult GetNotifiedExceptionClauseInfo(out COR_PRF_EX_CLAUSE_INFO pinfo)
    {
        return _impl.GetNotifiedExceptionClauseInfo(out pinfo);
    }
}