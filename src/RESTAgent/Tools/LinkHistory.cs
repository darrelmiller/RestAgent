using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tavis.Tools {
    public class LinkHistory : IEnumerable<Link>  {
        private readonly List<Link> _Links = new List<Link>();
        private const int _MaxStackSize = 10;

        public void AddToHistory(Link link) {
            _Links.Add(link);
            if (_Links.Count > _MaxStackSize) {
                _Links.RemoveAt(0);
            }
        }

        public Link CurrentLocation {
            get { return _Links.LastOrDefault(); }
        }

		public Link PreviousLocation {
			get { return _Links.Count == 0 ? null : _Links.Last(); }
        }

		public void GoBack() {
			_Links.RemoveAt(_Links.Count - 1);
		}

        public IEnumerator<Link> GetEnumerator() {
            return _Links.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
