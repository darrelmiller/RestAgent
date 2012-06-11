﻿using System;
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


        


        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, IFormatterLogger formatterLogger)
        {
            return new TaskFactory<object>().StartNew(() =>
                                                          {
                                                              return new StreamReader(stream).ReadToEnd();
                                                          });
        }

        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, TransportContext transportContext)
        {
            return new TaskFactory().StartNew(() =>
                                                          {
                                                              new StreamWriter(stream).Write((string)value);
                                                          });

            
        }

        public override bool CanReadType(Type type)
        {
            return type.Name == "System.String";
        }

        public override bool CanWriteType(Type type)
        {
            return type.Name == "System.String";
        }
    }
}
