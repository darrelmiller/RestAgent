using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tavis.Fakes {

    public class FakeWebRequestChannel : HttpMessageHandler {
        private readonly List<HttpResponseMessage> _ResponseMessages = new List<HttpResponseMessage>();

        public FakeWebRequestChannel(HttpContent content)  {
            var response = new HttpResponseMessage()
            {
                Content = content,
                StatusCode = HttpStatusCode.OK
            };
            _ResponseMessages.Add(response);
        }

        public FakeWebRequestChannel(List<HttpResponseMessage> responseMessages)
        {
            _ResponseMessages = responseMessages;
        }

        public FakeWebRequestChannel(HttpResponseMessage responseMessage) : this(new List<HttpResponseMessage>() {responseMessage}) {
            
        }


        private HttpResponseMessage Send(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) {

            var response = _ResponseMessages[0];
            _ResponseMessages.Remove(response);
            response.RequestMessage = request;
            return response;
        }


		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(() => Send(request, cancellationToken));
		}
    }
}
