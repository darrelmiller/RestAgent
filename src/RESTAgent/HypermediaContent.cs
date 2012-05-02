using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Tavis.Tools;

namespace Tavis {
    public class HypermediaContent {

		
        private readonly SemanticsRegistry _SemanticsRegistry;
		
        private ILinkExtractor _LinkExtractor;
        public object Value { get; private set; }
        public HttpContentHeaders Headers { get; private set; }


		public string MediaType {
			get { return Headers.ContentType.MediaType; }
        }


		public HypermediaContent(SemanticsRegistry semanticsRegistry, HttpContent content, HttpResponseHeaders responseHeaders = null)
		{
		    _SemanticsRegistry = semanticsRegistry;
		    Headers = content.Headers;
    	    Value = ReadContent(content);
		}


        public T GetLink<T>(string anchor = null) where T : Link {
			
    		var rel = _SemanticsRegistry.GetRel(typeof(T));
            return GetLink(rel, anchor) as T;
        }


    	public Link GetLink(string relation, string anchor = null) {
            return LinkExtractor.GetLink((r) => _SemanticsRegistry.CreateLink(r), Value, relation, anchor);
    	}


		public IEnumerable<T> GetLinks<T>(string anchor = null) where T : Link
		{
            var rel = _SemanticsRegistry.GetRel(typeof(T));

    		return  GetLinks(rel, anchor).OfType<T>();
        }


        public IEnumerable<Link> GetLinks(string relation = null, string anchor = null) {
            return LinkExtractor.GetLinks((r) => _SemanticsRegistry.CreateLink(r), Value, relation, anchor).ToList();
        }


        private ILinkExtractor LinkExtractor
        {
            get { return _LinkExtractor ?? (_LinkExtractor = _SemanticsRegistry.GetLinkExtractor(Value.GetType())); }
        }



		private MediaTypeFormatter GetFormatter(string mediaType)
		{
			var formatter = _SemanticsRegistry.Formatters.FirstOrDefault(
				f => f.SupportedMediaTypes.Select(m => m.MediaType).Contains(mediaType)
			);

			return formatter;
		}



		private object ReadContent(HttpContent content)
		{
			MediaTypeFormatter formatter = GetFormatter(MediaType);

            if (formatter != null) {
                var objectContent = new SimpleObjectContent<object>(content, formatter);
                return objectContent.ReadAsync().Result;
            } else {
                var stream = new MemoryStream();
                content.CopyToAsync(stream).RunSynchronously();
                stream.Position = 0;
                return stream;

            }
		}
    }
}
