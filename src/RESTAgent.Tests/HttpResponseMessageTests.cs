//using System;
//using System.Net.Http;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Tavis.Http.Tests
//{
//    [Ignore]		// Failing, can't fix (not our code)
//    [TestClass]
//    public class HttpResponseMessageTests
//    {
//        [TestMethod]
//        public void ReadingContentTypeIsNotThreadSafe()
//        {
//            // Arrange
//            // Create our own server/host so that the HttpResponseMessage for requests is
//            // newed up the usual internal way from a raw stream
//            var serviceUri = new Uri("http://localhost:1017/");
			

//            var host = new HttpServiceHost(typeof(FooService), new[] { serviceUri });
//            host.Open();

//            const int maxRuns = 50;

//            var httpClient = new HttpClient();
//            httpClient.BaseAddress = serviceUri;

//            var tasks = new List<Task>();
//            var runs = 0;

//            // Act
//            do {
//                var response = httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, "foo")).Result;

//                tasks.Clear();

//                for (var j = 0; j < 5; ++j) {
//                    tasks.Add(Task.Factory.StartNew(() => {
//                        for (var k = 0; k < 100000; ++k) {
//                            var contentType = response.Content.Headers.ContentType;
//                        }
//                    }));
//                }

//                Task.WaitAll(tasks.ToArray());

//                ++runs;
//                if (runs >= maxRuns) {
//                    break;
//                }
//            }
//            while (tasks.All(t => t.Exception == null));

//            host.Close();

//            // Assert
//            foreach (var task in tasks) {
//                if (task.Exception != null) {
//                    Assert.Fail(task.Exception.Message);
//                }
//            }
//        }


//        [ServiceContract]
//        private class FooService
//        {
//            [WebGet(UriTemplate = "foo")]
//            public HttpResponseMessage GetFoo()
//            {
//                var content = new StringContent("Blah blah blah");
//                var response = new HttpResponseMessage { Content = content };

//                return response;
//            }
//        }
//    }
//}
