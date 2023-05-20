namespace ProfilerLib;

public class ICorProfilerInfo10 : ICorProfilerInfo9
{
    private NativeObjects.ICorProfilerInfo10Invoker _impl;

    public ICorProfilerInfo10(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public unsafe HResult EnumerateObjectReferences(ObjectId objectId, delegate*unmanaged<ObjectId, ObjectId*, void*, int> callback, void* clientData)
    {
        return _impl.EnumerateObjectReferences(objectId, callback, clientData);
    }

    public HResult IsFrozenObject(ObjectId objectId, out int pbFrozen)
    {
        return _impl.IsFrozenObject(objectId, out pbFrozen);
    }

    public HResult GetLOHObjectSizeThreshold(out int pThreshold)
    {
        return _impl.GetLOHObjectSizeThreshold(out pThreshold);
    }

    public unsafe HResult RequestReJITWithInliners(int dwRejitFlags, uint cFunctions, ModuleId* moduleIds, MdMethodDef* methodIds)
    {
        return _impl.RequestReJITWithInliners(dwRejitFlags, cFunctions, moduleIds, methodIds);
    }

    public HResult SuspendRuntime()
    {
        return _impl.SuspendRuntime();
    }

    public HResult ResumeRuntime()
    {
        return _impl.ResumeRuntime();
    }
}