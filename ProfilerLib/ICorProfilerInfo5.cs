namespace ProfilerLib;

public class ICorProfilerInfo5 : ICorProfilerInfo4
{
    private NativeObjects.ICorProfilerInfo5Invoker _impl;

    public ICorProfilerInfo5(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult GetEventMask2(out COR_PRF_MONITOR pdwEventsLow, out COR_PRF_HIGH_MONITOR pdwEventsHigh)
    {
        return _impl.GetEventMask2(out pdwEventsLow, out pdwEventsHigh);
    }

    public HResult SetEventMask2(COR_PRF_MONITOR dwEventsLow, COR_PRF_HIGH_MONITOR dwEventsHigh)
    {
        return _impl.SetEventMask2(dwEventsLow, dwEventsHigh);
    }
}