using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Tavis {
    public class SemanticsRegistry  {


        private readonly List<MediaTypeFormatter> _Formatters = new List<MediaTypeFormatter>();
		private readonly IDictionary<Type, ILinkExtractor> _LinkExtractors = new Dictionary<Type, ILinkExtractor>();
		private readonly IDictionary<string, Link> _Bookmarks = new Dictionary<string, Link>();
		private readonly IDictionary<Link, Func<HttpContent>> _PreregisteredStaticContent = new Dictionary<Link, Func<HttpContent>>(new LinkComparer());

        private readonly IDictionary<Type, string> _RelLookup = new Dictionary<Type, string>();
        private readonly IDictionary<string, Type> _LinkTypes = new Dictionary<string, Type>();
        private readonly List<Func<Link, Link>> _LinkProcessors = new List<Func<Link, Link>>();

        
        public IEnumerable<MediaTypeFormatter> Formatters {
			get { return _Formatters; }
		}



    	public void RegisterFormatter(MediaTypeFormatter formatter)
		{
			_Formatters.Add(formatter);
        }


        public void RegisterLinkExtractor(ILinkExtractor linkExtractor) 
		{
			
			_LinkExtractors.Add(linkExtractor.SupportedType, linkExtractor);
			
        }


    	public void RegisterBookmark(string key, Link bookmarkLink)
		{
            _Bookmarks[key] = bookmarkLink;
        }


        public void RegisterStaticContent(Link link, Func<HttpContent> contentSource)
		{
			_PreregisteredStaticContent[link] = contentSource;
        }




    	public ILinkExtractor GetLinkExtractor(Type type)
    	{
			if (!_LinkExtractors.ContainsKey(type)) {
				throw new ArgumentException("Link extractor not found for type :" + type.Name);
			}

			return _LinkExtractors[type];
    	}


    	public Link GetBookmark(string key)
    	{
			return _Bookmarks.ContainsKey(key) ? _Bookmarks[key] : null;
    	}


		public HttpContent GetPreregisteredContent(Link link)
    	{
			return _PreregisteredStaticContent.ContainsKey(link) ? _PreregisteredStaticContent[link]() : null;
    	}


        public Link CreateLink(string linkRelation)
        {
            Link link;
            if ((linkRelation != null) && (_LinkTypes.Keys.Contains(linkRelation)))
            {
                link = (Link)Activator.CreateInstance(_LinkTypes[linkRelation]);
            }
            else
            {
                link = new Link() { Relation = linkRelation };
            }

            foreach (var processor in _LinkProcessors)
            {
                link = processor(link);
            }

            return link;
        }

        public string GetRel(Type type)
        {
            return _RelLookup[type];
        }

        public void RegisterLinkType<T>(string relation) where T : Link
        {
            var type = typeof(T);

            if (_RelLookup.ContainsKey(type))
            {
                throw new ArgumentException("Cannot register two relations to same type");
                // Because HypermediaContent caches links based on a rel reversed-looked-up from the type
            }

            _LinkTypes.Add(relation, type);
            _RelLookup.Add(type, relation);
        }


        public void RegisterLinkProcessor(Func<Link, Link> processor)
        {
            _LinkProcessors.Add(processor);
        }



		private class LinkComparer : IEqualityComparer<Link>
		{
			public bool Equals(Link x, Link y)
			{
				return x.Target == y.Target && x.Relation == y.Relation && x.Method == y.Method;
			}


			public int GetHashCode(Link obj)
			{
				return obj.Target.GetHashCode() ^ obj.Relation.GetHashCode() ^ obj.Method.GetHashCode();
			}
		}
    }
}
