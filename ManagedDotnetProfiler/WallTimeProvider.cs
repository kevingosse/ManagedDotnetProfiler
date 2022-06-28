using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ManagedDotnetProfiler
{
    internal class WallTimeProvider : ISamplesProvider
    {
        private const string ThreadName = "DD.Profiler.WallTimeProvider.Thread";

        private static readonly TimeSpan CollectingPeriod = TimeSpan.FromMilliseconds(60);

        private CancellationToken _cancellationToken;
        private Thread _transformerThread;
        private ConcurrentQueue<RawWallTimeSample> _rawSamples = new();
        private ConcurrentQueue<Sample> _samples = new();
        private readonly FrameStore _frameStore;

        public WallTimeProvider(FrameStore frameStore)
        {
            _frameStore = frameStore;
        }

        public void Start()
        {
            _transformerThread = new Thread(ProcessSamples) { IsBackground = true, Name = ThreadName };
            _transformerThread.Start();
        }

        private void ProcessSamples()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(CollectingPeriod);
                Flush();
            }
        }

        private void Flush()
        {
            while (_rawSamples.TryDequeue(out var rawSample))
            {
                var sample = new Sample(rawSample.Timestamp);

                SetAppDomainDetails(rawSample, sample);
                SetThreadDetails(rawSample, sample);

                SetStack(rawSample, sample);

                OnTransformRawSample(rawSample, sample);

                Store(sample);
            }
        }

        private void Store(Sample sample)
        {
            _samples.Enqueue(sample);
        }

        private void OnTransformRawSample(RawWallTimeSample rawSample, Sample sample)
        {
            sample.AddValue(rawSample.Duration, SampleValue.WallTimeDuration);
        }

        private void SetStack(RawWallTimeSample rawSample, Sample sample)
        {
            foreach (var ip in rawSample.InstructionPointers)
            {
                var (isResolved, frame) = _frameStore.GetFrame((nint)ip);

                if (isResolved)
                {
                    sample.AddFrame(frame);
                }
            }
        }

        private void SetAppDomainDetails(RawWallTimeSample rawSample, Sample sample)
        {
            // TODO
        }

        private void SetThreadDetails(RawWallTimeSample rawSample, Sample sample)
        {
            if (rawSample.ThreadInfo == null)
            {
                sample.SetThreadId("<0> [# 0]");
                sample.SetThreadName("Managed thread (name unknown) [#0]");
                return;
            }

            var profId = rawSample.ThreadInfo.ProfilerThreadInfoId;
            var osId = rawSample.ThreadInfo.OsThreadId;

            sample.SetThreadId($"<{profId}> [#{osId}]");

            // TODO: thread name
            sample.SetThreadName("Managed thread (name unknown)");
        }

        public IEnumerable<Sample> GetSamples()
        {
            while (_samples.TryDequeue(out var sample))
            {
                yield return sample;
            }
        }

        public void Add(RawWallTimeSample rawSample)
        {
            _rawSamples.Enqueue(rawSample);
        }
    }
}
