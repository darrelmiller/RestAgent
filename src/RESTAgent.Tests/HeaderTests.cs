using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tavis {
    [TestClass]
    public class HeaderTests {

        [Ignore]
        [TestMethod]  // Fixed in real code
        public void UserAgentWithMultipleProductInfoShouldBeValid() {

            var request1 = new HttpRequestMessage();
            request1.Headers.UserAgent.Add(new ProductInfoHeaderValue("foo","1.0"));
            request1.Headers.UserAgent.Add(new ProductInfoHeaderValue("bar", "2.0"));
            
            var request2 = new HttpRequestMessage();
            request2.Headers.Add("User-Agent", request1.Headers.UserAgent.ToString());
        }


        [TestMethod]  
        public void SetRequestHeaders() {

            var request1 = new HttpRequestMessage();
            request1.Headers.UserAgent.Add(new ProductInfoHeaderValue("foo", "1.0"));
            request1.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-ca",0.8));
            request1.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("fr-ca"));
            request1.Headers.MaxForwards = 10;
            request1.Headers.IfModifiedSince = new DateTimeOffset(DateTime.Today);
            request1.Headers.Referrer = new Uri("http://example.org");

        }

        [TestMethod]
        public void SetResponseHeaders() {

            var response = new HttpResponseMessage();
            response.Headers.CacheControl = new CacheControlHeaderValue() 
                {
                    MaxAge = new TimeSpan(0, 0, 30, 0),
                    Public = true,
                    NoTransform = true
                };
            response.Headers.Vary.Add("Accept-Language");
                                  
        }

    
    }
}
