using System.Runtime;

namespace TestApp
{
    internal class GarbageCollectionTests
    {
        public static void Run()
        {
            GC.Collect(0);
            GC.Collect(1);
            GC.Collect(2, GCCollectionMode.Default, blocking: true); // reason == COR_PRF_GC_INDUCED

            // For some weird reason, the GC is not reported as induced if blocking is false
            // https://github.com/dotnet/runtime/issues/86541
            GC.Collect(2, GCCollectionMode.Default, blocking: false); // reason == COR_PRF_GC_OTHER

            var logs = Logs.Fetch().ToList();

            Logs.AssertContains(logs, "GarbageCollectionStarted - 0 - COR_PRF_GC_INDUCED - 1");
            Logs.AssertContains(logs, "GarbageCollectionStarted - 0, 1 - COR_PRF_GC_INDUCED - 1");
            Logs.AssertContains(logs, "GarbageCollectionStarted - 0, 1, 2, 3, 4 - COR_PRF_GC_INDUCED - 1");
            Logs.AssertContains(logs, "GarbageCollectionStarted - 0, 1, 2, 3, 4 - COR_PRF_GC_OTHER - 1");
            Logs.AssertContains(logs, "GarbageCollectionFinished - 0");
        }
    }
}
