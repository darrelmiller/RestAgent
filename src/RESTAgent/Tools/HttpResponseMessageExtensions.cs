using System.Linq;
using System.Net;
using System.Net.Http;

namespace Tavis.Tools {
    public static class HttpResponseMessageExtensions {

        static readonly HttpStatusCode[] _SuccessCodes = {            HttpStatusCode.OK,
                                                                      HttpStatusCode.Created,
                                                                      HttpStatusCode.Accepted,
                                                                      HttpStatusCode.NonAuthoritativeInformation,
                                                                      HttpStatusCode.NoContent,
                                                                      HttpStatusCode.ResetContent, 
                                                                      HttpStatusCode.PartialContent
                                                                  };

        private static readonly HttpStatusCode[] _ClientErrors = { 
                                                                      HttpStatusCode.BadRequest,
                                                                      HttpStatusCode.Unauthorized,
                                                                      HttpStatusCode.PaymentRequired,
                                                                      HttpStatusCode.Forbidden,
                                                                      HttpStatusCode.NotFound, 
                                                                      HttpStatusCode.MethodNotAllowed,
                                                                      HttpStatusCode.NotAcceptable,
                                                                      HttpStatusCode.ProxyAuthenticationRequired,
                                                                      HttpStatusCode.RequestTimeout,
                                                                      HttpStatusCode.Conflict,
                                                                      HttpStatusCode.Gone,
                                                                      HttpStatusCode.LengthRequired,
                                                                      HttpStatusCode.PreconditionFailed,
                                                                      HttpStatusCode.RequestEntityTooLarge,
                                                                      HttpStatusCode.RequestUriTooLong,
                                                                      HttpStatusCode.UnsupportedMediaType,
                                                                      HttpStatusCode.RequestedRangeNotSatisfiable,
                                                                      HttpStatusCode.ExpectationFailed
                                                                  };

        private static readonly HttpStatusCode[] _ServerErrors = { 
                                                                      HttpStatusCode.InternalServerError,
                                                                      HttpStatusCode.NotImplemented,
                                                                      HttpStatusCode.BadGateway,
                                                                      HttpStatusCode.ServiceUnavailable,
                                                                      HttpStatusCode.GatewayTimeout, 
                                                                      HttpStatusCode.HttpVersionNotSupported
                                                                  };


        // response.HasClientErrorStatus
        public static bool HasClientErrorStatus(this HttpResponseMessage response) {
            return _ClientErrors.Contains(response.StatusCode);
        }

        public static bool HasServerErrorStatus(this HttpResponseMessage response) {
            return _ServerErrors.Contains(response.StatusCode);
        }

        public static bool HasSuccessStatus(this HttpResponseMessage response) {
            return _SuccessCodes.Contains(response.StatusCode);
        }

    } 
}
