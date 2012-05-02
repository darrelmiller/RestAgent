using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavis.Fakes;
using Tavis.Tools;

namespace Tavis {
    [TestClass]
    public class RestAgentTests {

        
        
        public RestAgentTests() {
            
        }

        [TestInitialize]
        public void Init() {
            
        }


        [TestMethod]
        public void CreateRestAgent() {
            //Arrange
            
            
            //Act
            var agent = new RestAgent();

            //Assert
            Assert.IsNotNull(agent);
        }


        [TestMethod]
        public void ShouldReturnContentFromRootUrl() {
            //Arrange
            var httpClient = new HttpClient(new FakeWebRequestChannel(new StringContent("This is a test")));
            

			var agent = new RestAgent(httpClient);
			agent.SemanticsRegistry.RegisterFormatter(new PlainTextFormatter());
            
            //Act
            agent.NavigateTo(new Link() { Target = new Uri("http://example.org/") });

            //Assert
            var textContent = (string)agent.CurrentContent.Value;
            Assert.AreEqual("This is a test", textContent);
            Assert.IsNotNull(agent);
        }



		[TestMethod]
		public void TestErrorHandling()
		{
			// Arrange
			

			var fakeResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
			var fakeWebRequestChannel = new FakeWebRequestChannel(fakeResponse);
			
            var httpClient = new HttpClient(fakeWebRequestChannel);
			var agent = new RestAgent(httpClient);
			
			// Act
            agent.NavigateTo(new Link() { Target = new Uri("http://example.org/") });
			var content = agent.CurrentContent;

			// Assert
			Assert.IsNull(content);
			Assert.AreEqual(HttpStatusCode.InternalServerError, agent.CurrentStatusCode);
		}



        [TestMethod]
        public void TestRedirectHandling()
        {
            // Arrange

            var responses = new List<HttpResponseMessage>();
            var redirectResponse = new HttpResponseMessage(HttpStatusCode.Redirect);
            redirectResponse.Headers.Location = new Uri("http://example.org/redirectlocation");
            responses.Add(redirectResponse);

            var targetResponse = new HttpResponseMessage(HttpStatusCode.OK);
            responses.Add(targetResponse);

            var fakeWebRequestChannel = new FakeWebRequestChannel(responses);

            var httpClient = new HttpClient(fakeWebRequestChannel);
            var agent = new RestAgent(httpClient);

            // Act
            agent.NavigateTo(new Link() { Target = new Uri("http://example.org/") });
            var content = agent.CurrentContent;

            // Assert
            Assert.IsNull(content);
            Assert.AreEqual(HttpStatusCode.OK, agent.CurrentStatusCode);
            Assert.AreEqual("http://example.org/redirectlocation", agent.CurrentLocation.Target.AbsoluteUri);
        }
    }
}
