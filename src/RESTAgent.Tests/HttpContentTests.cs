using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tavis {
    [TestClass]
    public class HttpContentTests {
     
        [TestMethod]
        public void ShouldBeAbleToCreateStringContent() {

            //Arrange

            //Act
            var content = new StringContent("Hello World");

            //Assert
            Assert.IsNotNull(content);
            Assert.AreEqual("Hello World",content.ReadAsStringAsync().Result);

        }

        //[Ignore]
        //[TestMethod]
        //public void ShouldBeAbleToAllTypesOfContent() {


        //    var stringContent = new StringContent("Hello World");
        //    var streamContent = new StreamContent(new FileStream("c:\\foo.txt",FileMode.Open));
        //    var byteArrayContent = new ByteArrayContent(new byte[] {10,20,30});


        //    var foo = new Foo() {Name = "Bob"};
        //    var objectContent = new SimpleObjectContent<Foo>(foo);
        //    var objectContentAsJson = new SimpleObjectContent<Foo>(foo,new MediaTypeHeaderValue("application/json"));

        //    var formatters = new MediaTypeFormatterCollection() {new MyMediaTypeFormatter()};
        //    var objectContentAsMyMediaType = new SimpleObjectContent<Foo>(foo, 
        //            new MediaTypeHeaderValue("application/vnd.me.mediatype"),formatters);


        //    Assert.IsNotNull(stringContent);
        //    Assert.IsNotNull(streamContent);
        //    Assert.IsNotNull(byteArrayContent);
        //    Assert.IsNotNull(objectContent);
        //    Assert.IsNotNull(objectContentAsJson);
        //    Assert.IsNotNull(objectContentAsMyMediaType);

        //}

        [TestMethod]
        public void CreateContentHeaders() {

            var stringContent = new StringContent("Bonjour Montréal");

            stringContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            stringContent.Headers.ContentLanguage.Add("fr-ca");
            stringContent.Headers.Expires = new DateTimeOffset(DateTime.Today.AddDays(3));
            stringContent.Headers.LastModified = DateTime.Today;
            
        }


        //public class MyMediaTypeFormatter : MediaTypeFormatter {

            
        //}

        public class Foo {
            public string Name { get; set;}
        }

        [TestMethod]
        public void ShouldBeAbleToCreateStreamContent() {

            //Arrange
            var ms = new MemoryStream();
            var sr = new StreamWriter(ms);
            sr.Write("Hello World");
            sr.Flush();
            ms.Position = 0;
            //Act
            var content = new StreamContent(ms);

            //Assert
            Assert.IsNotNull(content);
            Assert.AreEqual("Hello World", content.ReadAsStringAsync().Result);
            Assert.AreEqual("Hello World", new StreamReader(content.ReadAsStreamAsync().Result).ReadToEnd());

        }
        

        [TestMethod]
        public void ShouldBeAbleToConvertStreamContentIntoXElementContent() {

            //Arrange
            var ms = new MemoryStream();
            var sr = new StreamWriter(ms);
            sr.Write("<Hello>World</Hello>");
            sr.Flush();
            ms.Position = 0;
            var content = new StreamContent(ms);
            //Act
            var xElement = new XElementContent(content);
            var result = xElement.ReadAsStringAsync().Result;

            //Assert
            Assert.IsNotNull(xElement);
            Assert.IsTrue(result.EndsWith("<Hello>World</Hello>"));
            
        }


        
        public HttpRequestMessage GetRequestMessage() {

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://example.org");
            request.Method = HttpMethod.Get;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

            return request;
        }

        [TestMethod]
        public void CreateResponseMessage() {

            var response = new HttpResponseMessage();
            response.RequestMessage = GetRequestMessage();
            response.StatusCode = HttpStatusCode.BadRequest;
            response.ReasonPhrase = "Missing required information in request body";

        }

        [TestMethod]
        public void CreateResponseMessageWithContent() {

            var response = new HttpResponseMessage();
            response.Content = new StringContent("Here is some content");
            response.StatusCode = HttpStatusCode.OK;
            

        }
    }


    public static class HttpContentExtensions {
        
//        public static XElementContent ReadAsXElement
    }
    public class XElementContent : HttpContent {
        private XElement _Element;

        public XElementContent(XElement element) {
            _Element = element;
        }
        public XElementContent(HttpContent content)
        {
            _Element = XElement.Load(content.ReadAsStreamAsync().Result);
        }


        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) {
            return new TaskFactory().StartNew(() => _Element.Save(stream));
        }

        //protected override void SerializeToStream(Stream stream, TransportContext context) {
        //    var textWriter = XmlWriter.Create(stream,new XmlWriterSettings(){OmitXmlDeclaration = true, Encoding = Encoding.ASCII});
        //    _Element.Save(textWriter);
        //    textWriter.Flush();
        //}

        protected override bool TryComputeLength(out long length) {
            length = -1;
            return false;
        }
    }


//   public class ObjectContent : HttpContent
//{
//       public ObjectContent() {
           
//       }
//    // Constructors
//    public ObjectContent(Type type, object obj) {
        
//    }
//    public ObjectContent(Type type, object obj, string mediaType) {
        
//    }
//    public ObjectContent(Type type, object obj, MediaTypeHeaderValue mediaType) {}
//    public ObjectContent(Type type, HttpContent content) {}
//    public ObjectContent(Type type, object obj, IEnumerable<MediaTypeFormatter> formatters) {}
//    public ObjectContent(Type type, object obj, string mediaType,  
//                                                IEnumerable<MediaTypeFormatter> formatters) {}
//    public ObjectContent(Type type, object obj, MediaTypeHeaderValue mediaType, 
//                                                IEnumerable<MediaTypeFormatter> formatters) {}
//    public ObjectContent(Type type, HttpContent content, 
//                                                IEnumerable<MediaTypeFormatter> formatters) {}

//    // Properties
//    public Type Type { get; set; }
//    public MediaTypeFormatterCollection Formatters { get; set;}

//    // Methods
//    public object ReadAsObject() {
//        return null;
//    }

//       public object ReadAsObjectOrDefault() {
//           return null;
//       }

//       public Task<object> ReadAsObjectAsync() {
//           return null;}
//    public Task<object> ReadAsObjectOrDefaultAsync() {
//        return null;}

//       protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) {
//           throw new NotImplementedException();
//       }

//       protected override void SerializeToStream(Stream stream, TransportContext context) {
//           throw new NotImplementedException();
//       }

//       protected override bool TryComputeLength(out long length) {
//           throw new NotImplementedException();
//       }
//}


//    public class ObjectContent<T> : ObjectContent
//{
//    // Constructors
//    public ObjectContent(T obj) {}
//    public ObjectContent(T obj, string mediaType) {}
//    public ObjectContent(T obj, MediaTypeHeaderValue mediaType) {}
//    public ObjectContent(HttpContent content) {}
//    public ObjectContent(T obj, IEnumerable<MediaTypeFormatter> formatters) {}
//    public ObjectContent(T obj, string mediaType, 
//                         IEnumerable<MediaTypeFormatter> formatters) {}
//    public ObjectContent(T obj, MediaTypeHeaderValue mediaType, 
//                         IEnumerable<MediaTypeFormatter> formatters) {}
//    public ObjectContent(HttpContent content, IEnumerable<MediaTypeFormatter> formatters) {}

//    // Properties
//    public new T ReadAsObject() {
//        return default(T);}
//    public new T ReadAsObjectOrDefault() {return default(T);}
//    public new Task<T> ReadAsObjectAsync() {return null;}
//    public new Task<T> ReadAsObjectOrDefaultAsync() {
//        return null;}
//}


//    public class HttpRequestMessage<T> : HttpRequestMessage 
//{
//    // Constructors
//    public HttpRequestMessage() {}
//    public HttpRequestMessage(T obj) {}
//    public HttpRequestMessage(T obj, IEnumerable<MediaTypeFormatter> formatters) {}
//    public HttpRequestMessage(T obj, HttpMethod method, Uri requestUri,            
//                              IEnumerable<MediaTypeFormatter> formatters) {}
//    // Properties
//    public new ObjectContent<T> Content { get; set; }
//}



//    public abstract class MediaTypeFormatter
//{
//    // Constructors
//    protected MediaTypeFormatter() {}

//    // Properties
//    public Collection<MediaTypeHeaderValue> SupportedMediaTypes { get; set;}
//    public Collection<MediaTypeMapping> MediaTypeMappings { get; set;}

//    // Methods
//    public bool CanReadAs(Type type, HttpContent content) {
//        return false;}

//        public bool CanReadAs<T>(HttpContent content) {
//            return false;}

//        public bool CanWriteAs(ObjectContent content, out MediaTypeHeaderValue mediaType) {
//            mediaType = new MediaTypeHeaderValue("foo");
//            return false;}
//    public bool CanWriteAs<T>(ObjectContent<T> content, out MediaTypeHeaderValue mediaType) {
//        mediaType = new MediaTypeHeaderValue("foo");
//        return false;}
//    public bool CanWriteAs(Type type, HttpRequestMessage request, 
//                           out MediaTypeHeaderValue mediaType) {
//        mediaType = new MediaTypeHeaderValue("foo");
//        return false;
//    }
//    public bool CanWriteAs<T>(HttpRequestMessage<T> content, 
//                              out MediaTypeHeaderValue mediaType) {
//        mediaType = new MediaTypeHeaderValue("foo");
//        return false;
//    }

//        public bool CanWriteAs(Type type, HttpResponseMessage request, 
//                           out MediaTypeHeaderValue mediaType) {
//            mediaType = new MediaTypeHeaderValue("foo");
//            return false;
//        }
//    public bool CanWriteAs<T>(HttpResponseMessage<T> content, 
//                              out MediaTypeHeaderValue mediaType) {
//        mediaType = new MediaTypeHeaderValue("foo");
//        return false; }

//    public bool IsMediaTypeSupported(string mediatype) {
//        return false;
//        }
//    public bool IsMediaTypeSupported(MediaTypeHeaderValue mediatype) { return false; }


//    // Virtual Methods
//    protected virtual bool CanSupportType(Type type) {
//        return false; }
//    protected virtual Task<object> OnReadFromStreamAsync(Type type, Stream stream, 
//                                  HttpContentHeaders contentHeaders, TransportContext context) {
//        return null;
//    }  
//    protected virtual Task OnWriteToStreamAsync(Type type, object obj, Stream stream, 
//                                  HttpContentHeaders contentHeaders, TransportContext context) {
//        return null;
//    }
//    // Abstract Methods
//        protected abstract object OnReadFromStream(Type type, Stream stream,
//                                                   HttpContentHeaders contentHeaders, TransportContext context);

//        protected abstract void OnWriteToStream(Type type, object obj, Stream stream,
//                                                HttpContentHeaders contentHeaders, TransportContext context);
//}


//    public class MediaTypeFormatterCollection : Collection<MediaTypeFormatter> {
        
//    }

//    public class HttpResponseMessage<T> : HttpResponseMessage {
//    // Constructors
//    public HttpResponseMessage(HttpStatusCode statusCode) {}
//    public HttpResponseMessage(T obj) {}
//    public HttpResponseMessage(T obj, HttpStatusCode statusCode) {}
//    public HttpResponseMessage(T obj, IEnumerable<MediaTypeFormatter> formatters) {}
//    public HttpResponseMessage(T obj, HttpStatusCode statusCode,  IEnumerable<MediaTypeFormatter> formatters)     {}
//    // Properties
//    public new ObjectContent<T> Content { get; set; }
//}


//    public abstract class MediaTypeMapping
//{
//    // Constructors
//    protected MediaTypeMapping(MediaTypeHeaderValue mediaType) {}
//    protected MediaTypeMapping(string mediaType) {}

//    // Properties
//    public MediaTypeHeaderValue MediaType { get; set;}

//    // Methods
//    public bool SupportsMediaType(HttpRequestMessage request) {
//        return false;}

//    public bool SupportsMediaType(HttpResponseMessage response) {
//        return false;}

//    // Abstract Methods
//        protected abstract bool OnSupportsMediaType(HttpRequestMessage request);

//        protected abstract bool OnSupportsMediaType(HttpResponseMessage response);
//}

}
