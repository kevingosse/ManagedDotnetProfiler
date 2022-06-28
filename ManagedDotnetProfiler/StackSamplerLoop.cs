using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal class StackSamplerLoop
    {
        private const string ThreadName = "DD.Profiler.StackSamplerLoop.Thread";
        private const int MaxThreadsPerIterationForWallTime = 5;

        private static readonly TimeSpan SamplingPeriod = TimeSpan.FromMilliseconds(9);

        private readonly ICorProfilerInfo4 _corProfilerInfo;
        private readonly ThreadsCpuManager _threadCpuManager;
        private readonly ManagedThreadList _managedThreadList;
        private readonly StackSamplerLoopManager _manager;
        private readonly StackFramesCollector _stackFramesCollector;
        private readonly WallTimeProvider _wallTimeProvider;

        private Thread _loopThread;
        private int _loopThreadOsId;
        private CancellationToken _cancellationToken;
        private ManagedThreadInfo _targetThread;
        private ManagedThreadList.Enumerator _walltimeEnumerator;

        public StackSamplerLoop(
            StackSamplerLoopManager manager,
            ICorProfilerInfo4 corProfilerInfo,
            ThreadsCpuManager threadsCpuManager,
            ManagedThreadList managedThreadList,
            StackFramesCollector stackFramesCollector,
            WallTimeProvider wallTimeProvider)
        {
            _manager = manager;
            _corProfilerInfo = corProfilerInfo;
            _threadCpuManager = threadsCpuManager;
            _managedThreadList = managedThreadList;
            _stackFramesCollector = stackFramesCollector;
            _walltimeEnumerator = _managedThreadList.GetEnumerator();
            _wallTimeProvider = wallTimeProvider;

            _loopThread = new Thread(MainLoop)
            {
                IsBackground = true,
                Name = ThreadName
            };

            _loopThread.Start();
            _wallTimeProvider = wallTimeProvider;
        }

        private void MainLoop()
        {
            var hr = _corProfilerInfo.InitializeCurrentThread();

            if (!hr.IsOK)
            {
                Console.WriteLine("ICorProfilerInfo4::InitializeCurrentThread did not complete successfully");
            }

            _loopThreadOsId = OpSysTools.GetThreadId();
            _threadCpuManager.Map(_loopThreadOsId, ThreadName);

            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(SamplingPeriod);
                    MainLoopIteration();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in StackSamplerLoop.MainLoop: " + ex);
                }
            }
        }

        private void MainLoopIteration()
        {
            WalltimeProfilingIteration();
        }

        private void WalltimeProfilingIteration()
        {
            var managedThreadsCount = _managedThreadList.Count;
            var sampledThreadsCount = Math.Min(managedThreadsCount, MaxThreadsPerIterationForWallTime);

            for (int i = 0; i < sampledThreadsCount && !_cancellationToken.IsCancellationRequested; i++)
            {
                if (_walltimeEnumerator.MoveNext())
                {
                    _targetThread = _walltimeEnumerator.Current!;

                    var thisSampleTimestamp = Stopwatch.GetTimestamp();
                    var previousSampleTimestamp = _targetThread.SetLastTimestamp(thisSampleTimestamp);
                    var duration = ComputeWallTime(thisSampleTimestamp, previousSampleTimestamp);

                    CollectOneThreadStackSample(_targetThread, thisSampleTimestamp, duration, ProfilingType.WallTime);
                }
            }
        }

        private void CollectOneThreadStackSample(ManagedThreadInfo threadInfo, long thisSampleTimestamp, long duration, ProfilingType profilingType)
        {
            var osThreadHandle = threadInfo.OsThreadHandle;

            if (osThreadHandle == IntPtr.Zero)
            {
                return;
            }

            if (!_manager.AllowStackWalk(threadInfo))
            {
                return;
            }

            _stackFramesCollector.PrepareForNextCollection();

            var unixTimestamp = DateTime.Now - DateTime.UnixEpoch;

            threadInfo.SetLastKnownUnixTimestamp((long)unixTimestamp.TotalSeconds, thisSampleTimestamp);

            ref var result = ref _stackFramesCollector.CollectStackSample(threadInfo);

            _manager.NotifyIterationFinished();

            if (result.Count > 0)
            {
                UpdateSnapshotInfos(ref result, duration, (long)unixTimestamp.TotalSeconds);
                result.DetermineAppDomain(threadInfo.ClrThreadId, _corProfilerInfo);
            }

            PersistStackSnapshotResults(ref result, threadInfo, profilingType);
        }

        private void PersistStackSnapshotResults(ref StackSnapshotBuffer result, ManagedThreadInfo threadInfo, ProfilingType profilingType)
        {
            if (result.Count == 0)
            {
                return;
            }

            if (profilingType == ProfilingType.WallTime)
            {
                var rawSample = new RawWallTimeSample
                {
                    Timestamp = result.UnixTimestamp,
                    AppDomainId = result.AppDomainId,
                    ThreadInfo = threadInfo,
                    Duration = result.Duration
                };

                rawSample.SetInstructionPointers(ref result);

                _wallTimeProvider.Add(rawSample);
            }
        }

        private void UpdateSnapshotInfos(ref StackSnapshotBuffer result, long duration, long unixTimestamp)
        {
            result.Duration = duration;
            result.UnixTimestamp = unixTimestamp;
        }

        private long ComputeWallTime(long currentTimestamp, long previousTimestamp)
        {
            if (previousTimestamp == 0)
            {
                return (long)SamplingPeriod.TotalMilliseconds * 1000 * 1000;
            }

            if (previousTimestamp > 0)
            {
                double duration = currentTimestamp - previousTimestamp;
                return (long)((duration / Stopwatch.Frequency) * 1000000000);
            }

            return (long)SamplingPeriod.TotalMilliseconds * 1000 * 1000;
        }
    }
}
