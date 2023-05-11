using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    internal class PInvokes
    {
        public class Win32
        {
            [DllImport("kernel32.dll")]
            public static extern uint GetCurrentThreadId();
        }

        [DllImport("ManagedDotnetProfiler.dll")]
        public static extern unsafe int FetchLastLog(char* buffer, int bufferSize);

        [DllImport("ManagedDotnetProfiler.dll")]
        public static extern bool GetThreadId(ulong expectedThreadId, int expectedOSId);
    }
}
