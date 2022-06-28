using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Datadog.PProf.Export;
using ManagedDotnetProfiler.PProf.Export;

namespace ManagedDotnetProfiler
{
    internal class PprofExporter
    {
        private PProfBuilder _builder = new();
        private PProfBuildSession _session;
        private HttpClient _httpClient;

        public PprofExporter()
        {
            StartSession();
        }

        public void StartSession()
        {
            _httpClient = new();

            _builder.SetSampleValueTypes(
                new PProfSampleValueType("wall", "nanoseconds"),
                new PProfSampleValueType("cpu", "nanoseconds"),
                new PProfSampleValueType("exception", "count"));
            _session = _builder.StartNewPProfBuildSession();

            _session.Timestamp = DateTime.Now;
            _session.Duration = TimeSpan.FromSeconds(1);
            _session.Period = 1;
        }

        public void Add(Sample sample)
        {
            _session.AddNextSample();

            foreach (var frame in sample.Callstack)
            {
                _session.TryAddLocationToLastSample(frame);
            }

            var labels = new List<PProfSampleLabel>();

            foreach (var label in sample.Labels)
            {
                labels.Add(new PProfSampleLabel(label.key, label.value));
            }

            _session.SetSampleLabels(labels);
            _session.SetSampleValues(sample.Values);
        }

        public void Export()
        {
            try
            {
                var request = new MultipartFormPostRequest(_httpClient, "http://127.0.0.1:8126/profiling/v1/input", new MemoryStream(), _ => { });

                const string TimestampFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
                DateTimeOffset startTimestamp = _session.Timestamp.ToUniversalTime();
                DateTimeOffset endTimestamp = (_session.Timestamp + _session.Duration).ToUniversalTime();

                request.AddPlainTextFormPart("start", startTimestamp.ToString(TimestampFormat));
                request.AddPlainTextFormPart("end", endTimestamp.ToString(TimestampFormat));

                request.AddPlainTextFormPart("version", "3");
                request.AddPlainTextFormPart("family", "dotnet");
                request.AddPlainTextFormPart("tags[]", $"language:dotnet");
                request.AddPlainTextFormPart("tags[]", $"profiler_version:1.0");
                request.AddPlainTextFormPart("tags[]", $"pid:{Environment.ProcessId}");

                // request.AddPlainTextFormPart("tags[]", $"host:{_ddDataTags_Host}");
                request.AddPlainTextFormPart("tags[]", $"service:TestService");
                request.AddPlainTextFormPart("tags[]", $"env:kevin");
                request.AddPlainTextFormPart("tags[]", $"version:test");

                request.AddPlainTextFormPart("tags[]", $"runtime_version:nativeaot");
                request.AddPlainTextFormPart("tags[]", $"runtime_platform:Windows_{Environment.OSVersion.Version.ToString()}_x64");
                request.AddPlainTextFormPart("tags[]", $"process_architecture:x64");

                const string PprofContentName = "auto.pprof";
                using (Stream datas = request.AddOctetStreamFormPart(name: $"data[{PprofContentName}]", filename: PprofContentName))
                {
                    using (var outs = new WriteOnlyStream(
                        new GZipStream(datas, CompressionLevel.Fastest),
                        leaveUnderlyingStreamOpenWhenDisposed: false))
                    {
                        _session.WriteProfileToStream(outs);
                    }
                }

                var response = request.Send();

                Console.WriteLine($"Export result: status code {response.StatusCode} - Error: {response.Error} - Payload: {response.Payload}");

            }
            finally
            {
                _session.Dispose();
            }

            StartSession();
        }
    }
}
