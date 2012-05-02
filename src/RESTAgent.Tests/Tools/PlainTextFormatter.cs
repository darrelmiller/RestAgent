using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Tavis.Tools {
    public class PlainTextFormatter : MediaTypeFormatter {
        public PlainTextFormatter() {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }


        protected override System.Threading.Tasks.Task<object> OnReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            return new TaskFactory<object>().StartNew(() =>
                                                          {
                                                              return new StreamReader(stream).ReadToEnd();
                                                          });
        }

        protected override System.Threading.Tasks.Task OnWriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            return new TaskFactory().StartNew(() =>
                                                          {
                                                              new StreamWriter(stream).Write((string)value);
                                                          });

            
        }
    }
}
