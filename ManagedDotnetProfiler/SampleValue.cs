namespace ManagedDotnetProfiler;

internal enum SampleValue
{
    // Wall time profiler
    WallTimeDuration = 0,

    // CPU time profiler
    CpuTimeDuration = 1,

    // Exception profiler
    ExceptionCount = 2,

    MaxValue = 2
}