using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal class ManagedThreadInfo
    {
        public ManagedThreadInfo(ThreadId clrThreadId)
        {
            ClrThreadId = clrThreadId;
        }

        public uint ProfilerThreadInfoId { get; private set; }
        public ThreadId ClrThreadId { get; }
        public int OsThreadId { get; private set; }
        public IntPtr OsThreadHandle { get; private set; }
        public string ThreadName { get; set; }

        private long _lastSampleHighPrecisionTimestampNanoseconds;
        private ulong _cpuConsumptionMilliseconds;
        private long _lastKnownSampleUnixTimeUtc;
        private long _highPrecisionNanosecsAtLastUnixTimeUpdate;

        private ulong _snapshotsPerformedSuccessCount;
        private ulong _snapshotsPerformedFailureCount;

        private ulong _deadlockTotalCount;
        private ulong _deadlockInPeriodCount;
        private ulong _deadlockDetectionPeriod;

        public SemaphoreSlim StackWalkLock { get; }= new(1);
        public bool IsDestroyed { get; set; }

        public long SetLastTimestamp(long newTimestamp)
        {
            var previous = _lastSampleHighPrecisionTimestampNanoseconds;
            _lastSampleHighPrecisionTimestampNanoseconds = newTimestamp;
            return previous;
        }

        public void SetLastKnownUnixTimestamp(long unixTimestamp, long highPrecisionTimestamp)
        {
            _lastKnownSampleUnixTimeUtc = unixTimestamp;
            _highPrecisionNanosecsAtLastUnixTimeUpdate = highPrecisionTimestamp;
        }

        public void SetOsInfo(int osThreadId, IntPtr osThreadHandle)
        {
            OsThreadId = osThreadId;
            OsThreadHandle = osThreadHandle;
        }
    }
}
