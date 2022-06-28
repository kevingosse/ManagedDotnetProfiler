using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManagedDotnetProfiler.PProf.Export
{
    internal class MultipartFormPostRequest
    {
        private const string DocumentTextEncodingName = "utf-8";
        private static readonly Encoding BoundaryEncoding = Encoding.ASCII;
        private static readonly Encoding DocumentTextEncoding = Encoding.UTF8;

        private static readonly byte[] PlainTextContentTypeBytes = DocumentTextEncoding.GetBytes(
                                            $"Content-Type: text/plain; charset={DocumentTextEncodingName}\r\n\r\n");

        private static readonly byte[] PlainTextContentDispositionBytes1 = DocumentTextEncoding.GetBytes(
                                            "Content-Disposition: form-data; name=\"");

        private static readonly byte[] PlainTextContentDispositionBytes2 = DocumentTextEncoding.GetBytes(
                                            "\"\r\n");

        private static readonly byte[] OctetStreamContentTypeBytes = DocumentTextEncoding.GetBytes(
                                            "Content-Type: application/octet-stream\r\n\r\n");

        private static readonly byte[] OctetStreamContentDispositionBytes1 = DocumentTextEncoding.GetBytes(
                                            "Content-Disposition: form-data; name=\"");

        private static readonly byte[] OctetStreamContentDispositionBytes2 = DocumentTextEncoding.GetBytes(
                                            "\"; filename=\"");

        private static readonly byte[] OctetStreamContentDispositionBytes3 = DocumentTextEncoding.GetBytes(
                                            "\"\r\n");

        private readonly string _url;
        private readonly Action<MemoryStream> _releaseContentBufferStreamForReuseDelegate;
        private HttpClient _httpPoster;
        private Stream _content;

        private List<KeyValuePair<string, string>> _customHeaders;

        private string _boundary;
        private byte[] _boundaryBytes;
        private byte[] _finalBoundaryBytes;

        public MultipartFormPostRequest(HttpClient httpPoster, string url, MemoryStream reuseableContent, Action<MemoryStream> releaseReuseableContent)
        {
            _httpPoster = httpPoster;
            _url = url;
            _releaseContentBufferStreamForReuseDelegate = releaseReuseableContent;

            if (reuseableContent != null)
            {
                reuseableContent.Position = 0;
                _content = reuseableContent;
            }
            else if (reuseableContent == null)
            {
                _content = new MemoryStream();
            }

            InitHttpPosterAgnosticData();
        }

        public void AddHeader(string name, string value)
        {
            _customHeaders.Add(new KeyValuePair<string, string>(name, value));
        }

        public void AddPlainTextFormPart(string name, string content)
        {
            Write(_boundaryBytes);

            Write(PlainTextContentDispositionBytes1);
            Write(DocumentTextEncoding.GetBytes(name));
            Write(PlainTextContentDispositionBytes2);

            Write(PlainTextContentTypeBytes);

            Write(DocumentTextEncoding.GetBytes(content));
        }

        public Stream AddOctetStreamFormPart(string name, string filename)
        {
            Write(_boundaryBytes);

            Write(OctetStreamContentDispositionBytes1);
            Write(DocumentTextEncoding.GetBytes(name));
            Write(OctetStreamContentDispositionBytes2);
            Write(DocumentTextEncoding.GetBytes(filename));
            Write(OctetStreamContentDispositionBytes3);

            Write(OctetStreamContentTypeBytes);

            WriteOnlyStream octetStream = new WriteOnlyStream(_content, leaveUnderlyingStreamOpenWhenDisposed: true);
            return octetStream;
        }

        public Response Send()
        {
            HttpClient httpClient = Interlocked.Exchange(ref _httpPoster, null);
            if (httpClient == null)
            {
                throw new InvalidOperationException("This request has already been sent.");
            }

            Write(_finalBoundaryBytes);
            _content.Position = 0;

            int statusCode = 0;
            string statusCodeString = null;
            string payload = null;
            Exception error = null;

            MemoryStream contentBufferStream = null;
            if (_content is MemoryStream memStream)
            {
                contentBufferStream = memStream;
            }

            HttpContent requestContent = (contentBufferStream != null && (contentBufferStream.Length < int.MaxValue - 1))
                            ? (HttpContent)new ByteArrayContent(contentBufferStream.GetBuffer(), 0, (int)_content.Length)
                            : (HttpContent)new StreamContent(_content);

            using (requestContent)
            {
                requestContent.Headers.Add("Content-Type", $"multipart/form-data; boundary=\"{_boundary}\"");
                requestContent.Headers.ContentLength = _content.Length;
                for (int i = 0; i < _customHeaders.Count; i++)
                {
                    KeyValuePair<string, string> headerInfo = _customHeaders[i];
                    requestContent.Headers.Add(headerInfo.Key, headerInfo.Value);
                }

                using (var request = new HttpRequestMessage(HttpMethod.Post, _url))
                {
                    request.Content = requestContent;

                    try
                    {
                        HttpResponseMessage response = httpClient.Send(request);

                        using (response)
                        {
                            statusCode = (int)response.StatusCode;
                            statusCodeString = response.StatusCode.ToString();

                            Stream payloadStream = response.Content.ReadAsStream();
                            using (payloadStream)
                            using (StreamReader payloadReader = new StreamReader(payloadStream))
                            {
                                payload = payloadReader.ReadToEnd();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                }
            }

            // Return the allocated buffer for future reuse:
            if (contentBufferStream != null && _releaseContentBufferStreamForReuseDelegate != null)
            {
                _content = null;
                _releaseContentBufferStreamForReuseDelegate(contentBufferStream);
            }

            return new Response(statusCode, statusCodeString, payload, error);
        }

        private void InitHttpPosterAgnosticData()
        {
            _customHeaders = new List<KeyValuePair<string, string>>(capacity: 5);

            _boundary = Guid.NewGuid().ToString("N");
            _boundaryBytes = BoundaryEncoding.GetBytes($"\r\n--{_boundary}\r\n");
            _finalBoundaryBytes = BoundaryEncoding.GetBytes($"\r\n--{_boundary}--\r\n");
        }

        private void Write(byte[] bytes)
        {
            _content.Write(bytes, 0, bytes.Length);
        }
    }

    public struct Response
    {
        public Response(int statusCode, string statusCodeString, string payload, Exception error)
        {
            this.StatusCode = statusCode;
            this.StatusCodeString = statusCodeString;
            this.Payload = payload;
            this.Error = error;
        }

        public int StatusCode { get; }
        public string StatusCodeString { get; }
        public string Payload { get; }
        public Exception Error { get; }
    }


    internal class WriteOnlyStream : Stream
    {
        private const string ObjectDisposedMessage = "This " + nameof(WriteOnlyStream) + " is already disposed.";

        private readonly bool _leaveUnderlyingStreamOpenWhenDisposed;
        private Stream _underlyingStream;
        private long _writtenBytes;

        public WriteOnlyStream(Stream underlyingStream, bool leaveUnderlyingStreamOpenWhenDisposed)
        {
            if (!underlyingStream.CanWrite)
            {
                throw new ArgumentException($"The specified {nameof(underlyingStream)} must support"
                                           + " writing, but its CanWrite property returned false.");
            }

            _leaveUnderlyingStreamOpenWhenDisposed = leaveUnderlyingStreamOpenWhenDisposed;
            _underlyingStream = underlyingStream;
            _writtenBytes = 0;
        }

        public bool LeaveUnderlyingStreamOpenWhenDisposed
        {
            get { return _leaveUnderlyingStreamOpenWhenDisposed; }
        }

        public long WrittenBytes
        {
            get { return _writtenBytes; }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Stream underlyingStream = _underlyingStream ?? throw new ObjectDisposedException(ObjectDisposedMessage);
            underlyingStream.Write(buffer, offset, count);
            _writtenBytes += count;
        }

        public override void Flush()
        {
            Stream underlyingStream = _underlyingStream ?? throw new ObjectDisposedException(ObjectDisposedMessage);
            underlyingStream.Flush();
        }

        protected override void Dispose(bool disposing)
        {
            Stream underlyingStream = Interlocked.Exchange(ref _underlyingStream, null);

            if (underlyingStream != null)
            {
                underlyingStream.Flush();

                if (!_leaveUnderlyingStreamOpenWhenDisposed)
                {
                    underlyingStream.Dispose();
                }
            }
        }
    }

}
