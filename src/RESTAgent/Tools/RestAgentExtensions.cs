using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Tavis.Tools {
    public static class RestAgentExtensions {

        public static void SetLanguage(this RestAgent restAgent, string language) {
            ((HttpRequestHeaders)restAgent.DefaultRequestHeaders).AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
        }

        public static void AddProductToUserAgent(this RestAgent restAgent, ProductHeaderValue product) {
            ((HttpRequestHeaders)restAgent.DefaultRequestHeaders).UserAgent.Add(new ProductInfoHeaderValue(product));
        }

        public static void SetCredentials(this RestAgent restAgent, string token) {
            var headers = restAgent.DefaultRequestHeaders;
            headers.Add("Authorization", token);
        }

        public static void SetBasicCredentials(this RestAgent restAgent, string userName, string password) {
            var credentialKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));
            restAgent.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentialKey);
        }

        public static void SetAcceptedMediaTypes(this RestAgent restAgent, IEnumerable<MediaTypeWithQualityHeaderValue> value) {
          
                restAgent.DefaultRequestHeaders.Accept.Clear();

                if (value == null) {
                    restAgent.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                } else {
                    foreach (var headerValue in value) {
                        restAgent.DefaultRequestHeaders.Accept.Add(headerValue);
                    }
                }
            }


        public static void AutoSetAcceptedMediaTypes(this RestAgent restAgent, bool allowUnknown = false) {

            var mediaTypes = restAgent.SemanticsRegistry.Formatters.SelectMany(f => f.SupportedMediaTypes).Select(mt => new MediaTypeWithQualityHeaderValue(mt.MediaType));

            if (allowUnknown) {
                mediaTypes = mediaTypes.Concat(new[] { new MediaTypeWithQualityHeaderValue("*/*") });
            }

            restAgent.SetAcceptedMediaTypes(mediaTypes);
        }


    
    }
}
