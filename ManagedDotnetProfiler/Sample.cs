using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal class Sample
    {
        private const string ThreadIdLabel = "thread id";
        private const string ThreadNameLabel = "thread name";
        private const string AppDomainNameLabel = "appdomain name";
        private const string ProcessIdLabel = "appdomain process id";
        private const string LocalRootSpanIdLabel = "local root span id";
        private const string SpanIdLabel = "span id";
        private const string ExceptionTypeLabel = "exception type";
        private const string ExceptionMessageLabel = "exception message";

        public List<(string key, string value)> Labels { get; } = new();
        public List<StackFrame> Callstack { get; } = new();
        public long[] Values { get; } = new long[(int)SampleValue.MaxValue + 1];

        public Sample(long timestamp)
        {
            Timestamp = timestamp;
        }

        public long Timestamp { get; }

        public void AddValue(long value, SampleValue index)
        {
            Values[(int)index] = value;
        }

        public void SetPid(string pid)
        {
            AddLabel(ProcessIdLabel, pid);
        }

        public void SetAppDomainName(string name)
        {
            AddLabel(AppDomainNameLabel, name);
        }

        public void SetThreadId(string id)
        {
            AddLabel(ThreadIdLabel, id);
        }

        public void SetThreadName(string name)
        {
            AddLabel(ThreadNameLabel, name);
        }

        private void AddLabel(string key, string value)
        {
            Labels.Add((key, value));
        }

        public void AddFrame(StackFrame frame)
        {
            Callstack.Add(frame);
        }
    }
}
