using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Tavis.Tools
{
    public class HeaderLinkExtractor : ILinkExtractor
    {

        private static readonly Regex LinkHeaderParser = new Regex(@"<(?<href>[^>]*)>(?:\s*;\s*rel\s*=\s*(['""])(?<rel>.*?)\1)?", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private  List<Link> _HeaderLinks;

        public Link GetLink(Func<string,Link> factory, object content, string relation, string anchor) {
          //TOFIX  if (_HeaderLinks == null) ExtractHeaderLinks((HttpResponseHeaders)content, factory);

            var headerLinks = _HeaderLinks.Where(l => (l.Relation ?? "").ToLowerInvariant() == (relation ?? "").ToLowerInvariant());
            if ((anchor == null) )
            {
                return headerLinks.FirstOrDefault();
            }


            return null;
        }

        public IEnumerable<Link> GetLinks(Func<string, Link> factory, object content, string relation, string anchor)
        {
            //TOFIX if (_HeaderLinks == null) ExtractHeaderLinks((HttpResponseHeaders)content, factory);

            if (anchor == null)
            {
                return _HeaderLinks.Where(l => relation == null || (l.Relation ?? "").ToLowerInvariant() == relation.ToLowerInvariant()).ToArray();
            }
            return null;
        }

        public Type SupportedType {
            get { return typeof (HttpRequestHeaders); }
        }

        private void ExtractHeaderLinks(HttpResponseHeaders responseHeaders, SemanticsRegistry linkFactory) {
            
            _HeaderLinks = new List<Link>();

            foreach (var header in responseHeaders.Where(h => h.Key.ToLowerInvariant() == "link"))
            {
                var links = header.Value.First();

                foreach (Match match in LinkHeaderParser.Matches(links))
                {
                    string relation = null;
                    if (match.Groups["rel"].Success)
                    {
                        relation = match.Groups["rel"].Value;
                    }


                    var link = linkFactory.CreateLink(relation);
                    link.Target = new Uri(match.Groups["href"].Value, UriKind.RelativeOrAbsolute);


                    _HeaderLinks.Add(link);
                }
            }
        }
    }
}
