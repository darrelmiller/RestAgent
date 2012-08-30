using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Tavis.Tools {
    public class SimpleObjectContent<T> : HttpContent
    {
        private readonly T _outboundInstance;
        private readonly HttpContent _inboundContent;
        private readonly MediaTypeFormatter _formatter;

        // Outbound constructor
        public SimpleObjectContent(T outboundInstance, MediaTypeFormatter formatter)
        {
            _outboundInstance = outboundInstance;
            _formatter = formatter;
        }

        //Inbound constructor
        public SimpleObjectContent(HttpContent inboundContent, MediaTypeFormatter formatter)
        {
            _inboundContent = inboundContent;
            _formatter = formatter;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            // FormatterContext is required by XmlMediaTypeFormatter, but is not used by WriteToStreamAsync of XmlMediaTypeFormatter!
            return _formatter.WriteToStreamAsync(typeof(T), _outboundInstance, stream, this, null);
        }

        public Task<T> ReadAsync()
        {
            return this.ReadAsStreamAsync()
                .ContinueWith<object>(streamTask => _formatter.ReadFromStreamAsync(typeof(T), streamTask.Result, _inboundContent,  null))
                .ContinueWith<T>(objectTask => (T)((Task<object>)(objectTask.Result)).Result);
        }


        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return _inboundContent.ReadAsStreamAsync();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1L;
            return false;
        }
    }
}