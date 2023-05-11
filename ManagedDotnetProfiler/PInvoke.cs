using System;
using System.Runtime.InteropServices;

namespace ManagedDotnetProfiler
{
    internal unsafe class PInvoke
    {
        [UnmanagedCallersOnly(EntryPoint = "GetThreadId")]
        public static bool GetThreadId(ulong expectedThreadId, int expectedOsId)
        {
            return CorProfiler.Instance.GetThreadId(expectedThreadId, expectedOsId);
        }

        [UnmanagedCallersOnly(EntryPoint = "FetchLastLog")]
        public static int FetchLastLog(char* buffer, int size)
        {
            if (!CorProfiler.Logs.TryDequeue(out var log))
            {
                return -1;
            }

            if (size >= log.Length)
            {
                log.CopyTo(new Span<char>(buffer, size));
            }
            else
            {
                log.AsSpan(0, size).CopyTo(new Span<char>(buffer, size));
            }

            return log.Length;
        }
    }
}
