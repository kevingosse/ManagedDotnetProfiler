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

        public unsafe (HResult result, string moduleName, nint baseLoadAddress, AssemblyId assemblyId) GetModuleInfo(ModuleId moduleId)
        {
            var result = GetModuleInfo(moduleId, Span<char>.Empty, out _, out var length, out _);

            if (!result.IsOK)
            {
                return (result, default, default, default);
            }

            Span<char> buffer = stackalloc char[(int)length];

            result = GetModuleInfo(moduleId, buffer, out var baseLoadAddress, out _, out var assemblyId);

            if (!result.IsOK)
            {
                return (result, default, default, default);
            }

            return (result, buffer.WithoutNullTerminator(), baseLoadAddress, assemblyId);
        }


        public unsafe HResult GetModuleInfo(ModuleId moduleId, Span<char> moduleName, out nint ppBaseLoadAddress, out uint pcchName, out AssemblyId pAssemblyId)
        {
            fixed (char* c = moduleName)
            {
                return _impl.GetModuleInfo(moduleId, out ppBaseLoadAddress, (uint)moduleName.Length, out pcchName, c, out pAssemblyId);
            }
        }

        public HResult GetModuleMetaData(ModuleId moduleId, CorOpenFlags dwOpenFlags, Guid riid, out IMetaDataImport metaDataImport)
        {
            var result = _impl.GetModuleMetaData(moduleId, dwOpenFlags, riid, out var ptr);
            metaDataImport = new(ptr);
            return result;
        }

        public unsafe HResult GetILFunctionBody(ModuleId moduleId, MdMethodDef methodId, out byte* ppMethodHeader, out uint pcbMethodSize)
        {
            return _impl.GetILFunctionBody(moduleId, methodId, out ppMethodHeader, out pcbMethodSize);
        }

        public unsafe HResult GetILFunctionBodyAllocator(ModuleId moduleId, out void* ppMalloc)
        {
            return _impl.GetILFunctionBodyAllocator(moduleId, out ppMalloc);
        }

        public HResult SetILFunctionBody(ModuleId moduleId, MdMethodDef methodid, byte pbNewILMethodHeader)
        {
            return _impl.SetILFunctionBody(moduleId, methodid, pbNewILMethodHeader);
        }

        public unsafe (HResult result, string appDomainName, ProcessId processId) GetAppDomainInfo(AppDomainId appDomainId)
        {
            var result = GetAppDomainInfo(appDomainId, Span<char>.Empty, out var length, out _);

            if (!result.IsOK)
            {
                return (result, default, default);
            }

            Span<char> buffer = stackalloc char[(int)length];

            result = GetAppDomainInfo(appDomainId, buffer, out _, out var processId);

            if (!result.IsOK)
            {
                return (result, default, default);
            }

            return (result, buffer.WithoutNullTerminator(), processId);
        }

        public unsafe HResult GetAppDomainInfo(AppDomainId appDomainId, Span<char> appDomainName, out uint pcchName, out ProcessId pProcessId)
        {
            fixed (char* c = appDomainName)
            {
                return _impl.GetAppDomainInfo(appDomainId, (uint)appDomainName.Length, out pcchName, c, out pProcessId);
            }
        }

        public unsafe HResult GetAssemblyInfo(AssemblyId assemblyId, Span<char> assemblyName, out uint pcchName, out AppDomainId pAppDomainId, out ModuleId pModuleId)
        {
            fixed (char* c = assemblyName)
            {
                return _impl.GetAssemblyInfo(assemblyId, (uint)assemblyName.Length, out pcchName, c, out pAppDomainId, out pModuleId);
            }
        }

        public unsafe (HResult result, string assemblyName, AppDomainId appDomainId, ModuleId moduleId) GetAssemblyInfo(AssemblyId assemblyId)
        {
            var result = GetAssemblyInfo(assemblyId, Span<char>.Empty, out var length, out _, out _);

            if (!result.IsOK)
            {
                return (result, default, default, default);
            }

            Span<char> buffer = stackalloc char[(int)length];

            result = GetAssemblyInfo(assemblyId, buffer, out _, out var appDomainId, out var moduleId);

            if (!result.IsOK)
            {
                return (result, default, default, default);
            }

            return (result, buffer.WithoutNullTerminator(), appDomainId, moduleId);
        }

        public HResult SetFunctionReJIT(FunctionId functionId)
        {
            return _impl.SetFunctionReJIT(functionId);
        }

        public HResult ForceGC()
        {
            return _impl.ForceGC();
        }

        public unsafe HResult SetILInstrumentedCodeMap(FunctionId functionId, bool fStartJit, uint cILMapEntries, CorIlMap* rgILMapEntries)
        {
            return _impl.SetILInstrumentedCodeMap(functionId, fStartJit, cILMapEntries, rgILMapEntries);
        }

        public unsafe HResult GetInprocInspectionInterface(out void* ppicd)
        {
            return _impl.GetInprocInspectionInterface(out ppicd);
        }

        public unsafe HResult GetInprocInspectionIThisThread(out void* ppicd)
        {
            return _impl.GetInprocInspectionIThisThread(out ppicd);
        }

        public HResult GetThreadContext(ThreadId threadId, out ContextId pContextId)
        {
            return _impl.GetThreadContext(threadId, out pContextId);
        }

        public HResult BeginInprocDebugging(bool fThisThreadOnly, out int pdwProfilerContext)
        {
            return _impl.BeginInprocDebugging(fThisThreadOnly, out pdwProfilerContext);
        }

        public HResult EndInprocDebugging(int dwProfilerContext)
        {
            return _impl.EndInprocDebugging(dwProfilerContext);
        }

        public unsafe HResult GetILToNativeMapping(FunctionId functionId, uint cMap, out uint pcMap, CorDebugIlToNativeMap* map)
        {
            return _impl.GetILToNativeMapping(functionId, cMap, out pcMap, map);
        }
    }
}
