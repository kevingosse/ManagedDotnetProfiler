using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler
{
    internal class SamplesAggregator
    {
        private const string ThreadName = "DD.Profiler.SamplesAggregator.WorkerThread";

        private static readonly TimeSpan ProcessingInterval = TimeSpan.FromSeconds(1);

        private Thread _worker;
        private readonly List<ISamplesProvider> _samplesProvider = new();

        private CancellationToken _cancellationToken;

        private PprofExporter _exporter;

        private DateTime _lastExport;

        private long _samplesCount;

        public SamplesAggregator(PprofExporter exporter)
        {
            _exporter = exporter;
        }

        public void Start()
        {
            _lastExport = DateTime.UtcNow;

            // _exporter.StartSession();

            _worker = new Thread(Work)
            {
                IsBackground = true,
                Name = ThreadName
            };

            _worker.Start();
        }

        public void Register(ISamplesProvider samplesProvider)
        {
            _samplesProvider.Add(samplesProvider);
        }

        private void Work()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(ProcessingInterval);
                    ProcessSamples();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured while processing samples: " + ex);
                }
            }
        }

        private void ProcessSamples()
        {
            foreach (var sample in CollectSamples())
            {
                if (sample.Callstack.Count > 0)
                {
                    _exporter.Add(sample);
                    _samplesCount++;
                }
            }

            if ((DateTime.UtcNow - _lastExport).TotalMinutes > 1)
            {
                Console.WriteLine($"Exporting {_samplesCount} samples");

                _samplesCount = 0;

                _exporter.Export();

                _lastExport = DateTime.UtcNow;
            }
        }

        private IEnumerable<Sample> CollectSamples()
        {
            foreach (var provider in _samplesProvider)
            {
                foreach (var sample in provider.GetSamples())
                {
                    yield return sample;
                }
            }
        }
    }
}
