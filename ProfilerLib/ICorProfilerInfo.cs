namespace ProfilerLib
{
    public class ICorProfilerInfo
    {
        private NativeObjects.ICorProfilerInfoInvoker _impl;

        public ICorProfilerInfo(IntPtr ptr)
        {
            _impl = new(ptr);
        }

        public HResult QueryInterface(in Guid guid, out nint ptr)
        {
            return _impl.QueryInterface(in guid, out ptr);
        }

        public int AddRef()
        {
            return _impl.AddRef();
        }

        public int Release()
        {
            return _impl.Release();
        }

        public HResult GetClassFromObject(ObjectId ObjectId, out ClassId pClassId)
        {
            return _impl.GetClassFromObject(ObjectId, out pClassId);
        }

        public HResult GetClassFromToken(ModuleId ModuleId, MdTypeDef typeDef, out ClassId pClassId)
        {
            return _impl.GetClassFromToken(ModuleId, typeDef, out pClassId);
        }

        public unsafe HResult GetCodeInfo(FunctionId FunctionId, out byte* pStart, out uint pcSize)
        {
            return _impl.GetCodeInfo(FunctionId, out pStart, out pcSize);
        }

        public HResult GetEventMask(out int pdwEvents)
        {
            return _impl.GetEventMask(out pdwEvents);
        }

        public HResult GetFunctionFromIP(byte ip, out FunctionId pFunctionId)
        {
            return _impl.GetFunctionFromIP(ip, out pFunctionId);
        }

        public HResult GetFunctionFromToken(ModuleId ModuleId, MdToken token, out FunctionId pFunctionId)
        {
            return _impl.GetFunctionFromToken(ModuleId, token, out pFunctionId);
        }

        public HResult GetHandleFromThread(ThreadId ThreadId, out nint phThread)
        {
            return _impl.GetHandleFromThread(ThreadId, out phThread);
        }

        public HResult GetObjectSize(ObjectId ObjectId, out uint pcSize)
        {
            return _impl.GetObjectSize(ObjectId, out pcSize);
        }

        public HResult IsArrayClass(ClassId ClassId, out CorElementType pBaseElemType, out ClassId pBaseClassId, out uint pcRank)
        {
            return _impl.IsArrayClass(ClassId, out pBaseElemType, out pBaseClassId, out pcRank);
        }

        public HResult GetThreadInfo(ThreadId ThreadId, out int pdwWin32ThreadId)
        {
            return _impl.GetThreadInfo(ThreadId, out pdwWin32ThreadId);
        }

        public HResult GetCurrentThreadId(out ThreadId pThreadId)
        {
            return _impl.GetCurrentThreadId(out pThreadId);
        }

        public HResult GetClassIdInfo(ClassId ClassId, out ModuleId pModuleId, out MdTypeDef pTypeDefToken)
        {
            return _impl.GetClassIdInfo(ClassId, out pModuleId, out pTypeDefToken);
        }

        public HResult GetFunctionInfo(FunctionId FunctionId, out ClassId pClassId, out ModuleId pModuleId, out MdToken pToken)
        {
            return _impl.GetFunctionInfo(FunctionId, out pClassId, out pModuleId, out pToken);
        }

        public HResult SetEventMask(CorPrfMonitor dwEvents)
        {
            return _impl.SetEventMask(dwEvents);
        }

        public unsafe HResult SetEnterLeaveFunctionHooks(void* pFuncEnter, void* pFuncLeave, void* pFuncTailcall)
        {
            return _impl.SetEnterLeaveFunctionHooks(pFuncEnter, pFuncLeave, pFuncTailcall);
        }

        public unsafe HResult SetFunctionIdMapper(void* pFunc)
        {
            return _impl.SetFunctionIdMapper(pFunc);
        }

        public unsafe HResult GetTokenAndMetaDataFromFunction(FunctionId FunctionId, out Guid riid, out void* ppImport, out MdToken pToken)
        {
            return _impl.GetTokenAndMetaDataFromFunction(FunctionId, out riid, out ppImport, out pToken);
        }

        public unsafe HResult GetModuleInfo(ModuleId ModuleId, out nint ppBaseLoadAddress, uint cchName, out uint pcchName, char* szName, out AssemblyId pAssemblyId)
        {
            return _impl.GetModuleInfo(ModuleId, out ppBaseLoadAddress, cchName, out pcchName, szName, out pAssemblyId);
        }

        public HResult GetModuleMetaData(ModuleId ModuleId, CorOpenFlags dwOpenFlags, Guid riid, out IMetaDataImport metaDataImport)
        {
            var result = _impl.GetModuleMetaData(ModuleId, dwOpenFlags, riid, out var ptr);
            metaDataImport = new(ptr);
            return result;
        }

        public unsafe HResult GetILFunctionBody(ModuleId ModuleId, MdMethodDef methodId, out byte* ppMethodHeader, out uint pcbMethodSize)
        {
            return _impl.GetILFunctionBody(ModuleId, methodId, out ppMethodHeader, out pcbMethodSize);
        }

        public unsafe HResult GetILFunctionBodyAllocator(ModuleId ModuleId, out void* ppMalloc)
        {
            return _impl.GetILFunctionBodyAllocator(ModuleId, out ppMalloc);
        }

        public HResult SetILFunctionBody(ModuleId ModuleId, MdMethodDef methodid, byte pbNewILMethodHeader)
        {
            return _impl.SetILFunctionBody(ModuleId, methodid, pbNewILMethodHeader);
        }

        public unsafe HResult GetAppDomainInfo(AppDomainId appDomainId, Span<char> appDomainName,out uint pcchName, out ProcessId pProcessId)
        {
            fixed (char* c = appDomainName)
            {
                return _impl.GetAppDomainInfo(appDomainId, (uint)appDomainName.Length, out pcchName, c, out pProcessId);
            }
        }

        public unsafe HResult GetAssemblyInfo(AssemblyId assemblyId, uint cchName, out uint pcchName, out char* szName, out AppDomainId pAppDomainId, out ModuleId pModuleId)
        {
            return _impl.GetAssemblyInfo(assemblyId, cchName, out pcchName, out szName, out pAppDomainId, out pModuleId);
        }

        public HResult SetFunctionReJIT(FunctionId functionId)
        {
            return _impl.SetFunctionReJIT(functionId);
        }

        public HResult ForceGC()
        {
            return _impl.ForceGC();
        }

        public unsafe HResult SetILInstrumentedCodeMap(FunctionId FunctionId, bool fStartJit, uint cILMapEntries, CorIlMap* rgILMapEntries)
        {
            return _impl.SetILInstrumentedCodeMap(FunctionId, fStartJit, cILMapEntries, rgILMapEntries);
        }

        public unsafe HResult GetInprocInspectionInterface(out void* ppicd)
        {
            return _impl.GetInprocInspectionInterface(out ppicd);
        }

        public unsafe HResult GetInprocInspectionIThisThread(out void* ppicd)
        {
            return _impl.GetInprocInspectionIThisThread(out ppicd);
        }

        public HResult GetThreadContext(ThreadId ThreadId, out ContextId pContextId)
        {
            return _impl.GetThreadContext(ThreadId, out pContextId);
        }

        public HResult BeginInprocDebugging(bool fThisThreadOnly, out int pdwProfilerContext)
        {
            return _impl.BeginInprocDebugging(fThisThreadOnly, out pdwProfilerContext);
        }

        public HResult EndInprocDebugging(int dwProfilerContext)
        {
            return _impl.EndInprocDebugging(dwProfilerContext);
        }

        public unsafe HResult GetILToNativeMapping(FunctionId FunctionId, uint cMap, out uint pcMap, CorDebugIlToNativeMap* map)
        {
            return _impl.GetILToNativeMapping(FunctionId, cMap, out pcMap, map);
        }
    }
}
