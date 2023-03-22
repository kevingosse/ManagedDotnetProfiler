namespace ProfilerLib;

[NativeObject]
public unsafe interface ICorProfilerCallback11 : ICorProfilerCallback10
{
    public static readonly Guid Guid = Guid.Parse("42350846-AAED-47F7-B128-FD0C98881CDE");

    HResult LoadAsNotificationOnly(out bool pbNotificationOnly);
}