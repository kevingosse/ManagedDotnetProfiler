namespace ProfilerLib;

public unsafe class ICorProfilerInfo13 : ICorProfilerInfo12
{
    private NativeObjects.ICorProfilerInfo13Invoker _impl;

    public ICorProfilerInfo13(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult<ObjectHandleId> CreateHandle(ObjectId @object, COR_PRF_HANDLE_TYPE type)
    {
        var result = _impl.CreateHandle(@object, type, out var handle);
        return new(result, handle);
    }

    public HResult DestroyHandle(ObjectHandleId handle)
    {
        return _impl.DestroyHandle(handle);
    }

    public HResult<ObjectId> GetObjectIDFromHandle(ObjectHandleId handle)
    {
        var result = _impl.GetObjectIDFromHandle(handle, out var @object);
        return new(result, @object);
    }
}