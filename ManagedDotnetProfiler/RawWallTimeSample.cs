using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal class RawWallTimeSample
    {
        public long Timestamp { get; set; }
        public AppDomainId AppDomainId { get; set; }
        public ManagedThreadInfo ThreadInfo { get; set; }
        public long Duration { get; set; }

        public long[] InstructionPointers { get; set; }

        public unsafe void SetInstructionPointers(ref StackSnapshotBuffer buffer)
        {
            InstructionPointers = new long[buffer.Count];

            fixed (long* source = buffer.InstructionPointers)
            {
                Marshal.Copy((IntPtr)source, InstructionPointers, 0, buffer.Count);
            }
        }
    }
}
