using System;

namespace Tavis.Tools {
    

    public static class UriExtensions {

        public static Uri AsBaseUrl(this Uri url) {
            var urlstring = url.OriginalString;
            return new Uri(urlstring.Substring(0,urlstring.LastIndexOf('/')));
        }
    }
    
}
