using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;

namespace Tavis.Fakes {

    public class FakeFormatter : MediaTypeFormatter {

        public readonly Dictionary<string, Link> Links = new Dictionary<string, Link>();

        protected override System.Threading.Tasks.Task<object> OnReadFromStreamAsync(Type type, Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            return base.OnReadFromStreamAsync(type, stream, contentHeaders, formatterContext);
        }

        protected override System.Threading.Tasks.Task OnWriteToStreamAsync(Type type, object value, Stream stream, System.Net.Http.Headers.HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            return base.OnWriteToStreamAsync(type, value, stream, contentHeaders, formatterContext, transportContext);
        }
    }


  
}
