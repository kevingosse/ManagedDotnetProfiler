namespace ProfilerLib;

public class ICorProfilerInfo5 : ICorProfilerInfo4
{
    private NativeObjects.ICorProfilerInfo5Invoker _impl;

    public ICorProfilerInfo5(IntPtr ptr) : base(ptr)
    {
        _impl = new(ptr);
    }

    public HResult GetEventMask2(out CorPrfMonitor pdwEventsLow, out CorPrfHighMonitor pdwEventsHigh)
    {
        return _impl.GetEventMask2(out pdwEventsLow, out pdwEventsHigh);
    }

    public HResult SetEventMask2(CorPrfMonitor dwEventsLow, CorPrfHighMonitor dwEventsHigh)
    {
        return _impl.SetEventMask2(dwEventsLow, dwEventsHigh);
    }
}