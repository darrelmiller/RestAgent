using System;
using System.Net;
using System.Net.Http;

namespace Tavis.Actions {
    public class RedirectAction : ResponseAction {
        private readonly RestAgent _RestAgent;
        private Uri _RedirectUrl;
        private HttpStatusCode _StatusCode;
        private Link _TargetLink;

        public RedirectAction(RestAgent restAgent) {
            _RestAgent = restAgent;
        }

        public override bool ShouldRespond(HttpResponseMessage response) {
            return response.StatusCode == HttpStatusCode.Redirect ||
                   response.StatusCode == HttpStatusCode.RedirectMethod;
        }

        public override void HandleResponseStart(Link link, HttpResponseMessage response) {
            _RedirectUrl = response.Headers.Location;
            _StatusCode = response.StatusCode;
            _TargetLink = link;
        }
        public override void HandleResponseComplete(HypermediaContent currentContent) {
            var link = new Link() { Target = _RedirectUrl };
            if (_StatusCode == HttpStatusCode.RedirectMethod) {
                link.Method = _TargetLink.Method;
            }
            _RestAgent.NavigateTo(link);
        }
    }
}