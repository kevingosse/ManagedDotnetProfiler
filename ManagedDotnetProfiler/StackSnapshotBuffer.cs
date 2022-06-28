using System.Text;

namespace ManagedDotnetProfiler
{
    internal unsafe struct StackSnapshotBuffer
    {
        public const int Size = 1024;

        public int Count { get; private set; }

        public long Duration { get; set; }

        public long UnixTimestamp { get; set; }

        public AppDomainId AppDomainId { get; set; }

        public fixed long InstructionPointers[1024];

        public void Reset()
        {
            Count = 0;
        }

        public bool Add(nint ip)
        {
            if (Count >= Size)
            {
                return false;
            }

            InstructionPointers[Count++] = ip;

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{Count} frames:");

            for (int i = 0; i < Count; i++)
            {
                sb.AppendLine($" - {InstructionPointers[i]:x2}");
            }

            return sb.ToString();
        }

        public void DetermineAppDomain(ThreadId threadId, ICorProfilerInfo4 corProfilerInfo)
        {
            var result = corProfilerInfo.GetThreadAppDomain(threadId, out var appDomainId);

            if (result.IsOK)
            {
                AppDomainId = appDomainId;
            }
        }
    }
}
