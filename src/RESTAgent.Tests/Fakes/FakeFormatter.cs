using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;

namespace Tavis.Fakes {

    public class FakeFormatter : MediaTypeFormatter {

        public readonly Dictionary<string, Link> Links = new Dictionary<string, Link>();




        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, Stream stream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            return base.WriteToStreamAsync(type, value, stream, content, transportContext);
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(Type type, Stream stream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            return base.ReadFromStreamAsync(type, stream, content, formatterLogger);
        }
    }


  
}
