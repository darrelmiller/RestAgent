using System.Net.Http;

namespace Tavis.Actions {
    public abstract class ResponseAction {

        public abstract bool ShouldRespond(HttpResponseMessage responseMessage);

        public abstract void HandleResponseComplete(HypermediaContent currentContent);

        public abstract void HandleResponseStart(Link link, HttpResponseMessage response);
    }
}