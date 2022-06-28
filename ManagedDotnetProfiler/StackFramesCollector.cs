using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal class StackFramesCollector
    {
        private ManagedThreadInfo _currentCollectionThreadInfo;
        private bool _isCurrentCollectionAbortRequested;
        private bool _isRequestedCollectionAbortSuccessful;
        private readonly ICorProfilerInfo4 _corProfilerInfo;
        private GCHandle _bufferHandle;

        public StackFramesCollector(ICorProfilerInfo4 corProfilerInfo)
        {
            _corProfilerInfo = corProfilerInfo;
            _bufferHandle = GCHandle.Alloc(new StackSnapshotBuffer(), GCHandleType.Pinned);
        }

        public void PrepareForNextCollection()
        {
            GetBuffer().Reset();
            _currentCollectionThreadInfo = null;
            _isCurrentCollectionAbortRequested = false;
            _isRequestedCollectionAbortSuccessful = false;
        }

        public ref StackSnapshotBuffer CollectStackSample(ManagedThreadInfo threadInfo)
        {
            _currentCollectionThreadInfo = threadInfo;

            var currentThreadId = OpSysTools.GetThreadId();

            CollectStackSampleImplementation(threadInfo, threadInfo.OsThreadId == currentThreadId);

            return ref GetBuffer();
        }

        private unsafe ref StackSnapshotBuffer GetBuffer()
        {
            return ref Unsafe.AsRef<StackSnapshotBuffer>((void*)_bufferHandle.AddrOfPinnedObject());
        }

        private unsafe void CollectStackSampleImplementation(ManagedThreadInfo threadInfo, bool selfCollect)
        {
            _corProfilerInfo.DoStackSnapshot(
                selfCollect ? default : threadInfo.ClrThreadId,
                &StackSnapshotCallback,
                COR_PRF_SNAPSHOT_INFO.COR_PRF_SNAPSHOT_DEFAULT,
                (void*)_bufferHandle.AddrOfPinnedObject(),
                null,
                0);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
        private static unsafe HResult StackSnapshotCallback(FunctionId functionId, nint ip, COR_PRF_FRAME_INFO frameInfo, uint contextSize, byte* context, void* clientData)
        {
            ref var buffer = ref Unsafe.AsRef<StackSnapshotBuffer>(clientData);

            return buffer.Add(ip) ? HResult.S_OK : HResult.E_FAIL;
        }
    }
}
