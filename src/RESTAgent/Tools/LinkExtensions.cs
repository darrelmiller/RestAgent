using System;

namespace Tavis.Tools
{
	public static class LinkExtensions
	{
		public static void MakeAbsolute(this Link link, Uri baseUri)
		{
			if (link == null) throw new ArgumentNullException("link");

			if (!link.Target.IsAbsoluteUri) {
				if (baseUri == null) throw new ArgumentNullException("baseUri");
				link.Target = new Uri(baseUri, link.Target.OriginalString);
			}
		}

		public static void MakeAbsolute(this Link link, Link baseLink)
		{
			if (baseLink != null) {
				link.MakeAbsolute(baseLink.Target);
			}
			else {
				link.MakeAbsolute((Uri)null);
			}
		}
	}
}
