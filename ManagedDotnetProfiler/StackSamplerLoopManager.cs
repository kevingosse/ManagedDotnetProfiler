using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal class StackSamplerLoopManager
    {
        private object _watcherActivityLock = new();

        private ManagedThreadInfo _targetThread;
        private bool _isTargetThreadSuspended;
        private bool _isForceTerminated;
        private StackSamplerLoop _stackSamplerLoop;
        private readonly ICorProfilerInfo4 _corProfilerInfo;
        private readonly ThreadsCpuManager _threadsCpuManager;
        private readonly ManagedThreadList _managedThreadList;
        private readonly StackFramesCollector _stackFramesCollector;
        private readonly WallTimeProvider _wallTimeProvider;

        public StackSamplerLoopManager(
            ICorProfilerInfo4 corProfilerInfo,
            ThreadsCpuManager threadsCpuManager,
            ManagedThreadList managedThreadList,
            StackFramesCollector stackFramesCollector,
            WallTimeProvider wallTimeProvider)
        {
            _corProfilerInfo = corProfilerInfo;
            _threadsCpuManager = threadsCpuManager;
            _managedThreadList = managedThreadList;
            _stackFramesCollector = stackFramesCollector;
            _wallTimeProvider = wallTimeProvider;
        }

        public void Start()
        {
            _stackSamplerLoop = new StackSamplerLoop(
                this,
                _corProfilerInfo,
                _threadsCpuManager,
                _managedThreadList,
                _stackFramesCollector,
                _wallTimeProvider);
        }

        public bool AllowStackWalk(ManagedThreadInfo threadInfo)
        {
            // TODO: check thread status

            threadInfo.StackWalkLock.Wait();

            if (threadInfo.IsDestroyed)
            {
                threadInfo.StackWalkLock.Release();
                return false;
            }

            _targetThread = threadInfo;
            _isTargetThreadSuspended = false;
            _isForceTerminated = false;

            return true;
        }

        public void NotifyIterationFinished()
        {
            _targetThread.StackWalkLock.Release();
        }
    }
}
