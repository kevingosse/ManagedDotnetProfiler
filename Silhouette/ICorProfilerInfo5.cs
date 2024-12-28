namespace Silhouette;

public class ICorProfilerInfo5 : ICorProfilerInfo4
{
    private NativeObjects.ICorProfilerInfo5Invoker _impl;

    public ICorProfilerInfo5(nint ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult<EventMask2> GetEventMask2()
    {
        var result = _impl.GetEventMask2(out var eventsLow, out var eventsHigh);
        return new(result, new(eventsLow, eventsHigh));
    }

    public HResult SetEventMask2(COR_PRF_MONITOR dwEventsLow, COR_PRF_HIGH_MONITOR dwEventsHigh)
    {
        return _impl.SetEventMask2(dwEventsLow, dwEventsHigh);
    }
}