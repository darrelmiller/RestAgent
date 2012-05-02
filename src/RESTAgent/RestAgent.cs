using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tavis.Actions;
using Tavis.Tools;

namespace Tavis {
    public class RestAgent : IDisposable {

        public event Action AgentTerminated = delegate { };

        private readonly HttpClient _HttpClient;
        private readonly SemanticsRegistry _SemanticsRegistry;
        private readonly Dictionary<string, HttpContent> _ContentStore = new Dictionary<string, HttpContent>();
        private readonly Dictionary<string, Link> _Bookmarks = new Dictionary<string, Link>();
        private readonly LinkHistory _LinkHistory = new LinkHistory();
        private readonly List<ResponseAction> _ResponseActions = new List<ResponseAction>();

        private RestAgent _ChildAgent;
        private HypermediaContent _CurrentContent;
        private HttpStatusCode _CurrentStatusCode = HttpStatusCode.OK;


        public RestAgent(Uri startUrl) : this(CreateHttpClient(), new SemanticsRegistry()) {
            var startLink = new Link() {Target = startUrl};
            _HttpClient.BaseAddress = startLink.Target;
            NavigateTo(startLink);
        }

        public RestAgent()
            : this(CreateHttpClient(), new SemanticsRegistry())
        {
        }

        public RestAgent(HttpClient httpClient)
            : this(httpClient, new SemanticsRegistry())
        {
        }


        private RestAgent(HttpClient httpClient, SemanticsRegistry semanticsRegistry) {
            _HttpClient = httpClient;
            _SemanticsRegistry = semanticsRegistry;
            CreateDefaultResponseActions();
        }


        public SemanticsRegistry SemanticsRegistry {
            get { return _SemanticsRegistry; }
        }

        public HttpRequestHeaders DefaultRequestHeaders {
            get { return _HttpClient.DefaultRequestHeaders; }
        }


        public HttpStatusCode CurrentStatusCode {
            get { return _CurrentStatusCode; }
        }

        public HypermediaContent CurrentContent {
            get {
                    return _CurrentContent;
            }
        }

        public Link CurrentLocation {
            get { return _LinkHistory.CurrentLocation; }
        }

        public bool CanNavigatePrevious {
            get { return _LinkHistory.PreviousLocation != null; }
        }


 public void RegisterResponseAction(ResponseAction responseAction) {
            _ResponseActions.Add(responseAction);
        }


        public HttpContent GetContent(string label) {
            return _ContentStore[label];
        }


        public void CreateBookmark(string bookmark, Link link) {
            _Bookmarks.Add(bookmark, link);
        }

        public Link RetreiveBookmark(string bookmarkName) {
            return (Link)_SemanticsRegistry.GetBookmark(bookmarkName) ?? _Bookmarks[bookmarkName];
        }


        public Task EmbedContent(string label, Link link) {
            return GetContentAsync(link).ContinueWith((t)=>_ContentStore[label] = t.Result);
        }

        public Task[] EmbedContent(IEnumerable<Link> links) {
            var tasks = new List<Task<HttpContent>>();

            foreach (var link in links) {
                var label = link.Target.OriginalString;
                var task = GetContentAsync(link);
                task.ContinueWith(t => _ContentStore[label] = t.Result);

                tasks.Add(task);
            }

            return tasks.ToArray();
        }


        public RestAgent NavigateToWithChildAgent(Link link) {
            _ChildAgent = new RestAgent(_HttpClient, _SemanticsRegistry);
            _ChildAgent._ResponseActions.AddRange(_ResponseActions);

            _ChildAgent.InternalNavigateTo(link);
            _ChildAgent.AgentTerminated += () => _ChildAgent = null;
            return _ChildAgent;
        }

        public void NavigateTo(Link link)
        {
            InternalNavigateTo(link);
        }

        public void NavigateToPrevious()
        {
            if (!CanNavigatePrevious)
            {
                throw new InvalidOperationException("Nowhere to go back to");
            }

            var link = _LinkHistory.PreviousLocation;

            InternalNavigateTo(link);
        }

        public void Dispose()
        {
            if (_ChildAgent != null)
            {
                _ChildAgent.Dispose();
            }

            AgentTerminated();

            _HttpClient.Dispose();
        }


        

        private void InternalNavigateTo(Link link) {

            link.MakeAbsolute(CurrentLocation);

            HttpResponseMessage response;

            var content = _SemanticsRegistry.GetPreregisteredContent(link);
            if (content != null) {
                response = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
            } else {
                HttpRequestMessage request = link.CreateRequest();
                response = _HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result;
            }

            HandleResponse(response, link);
        }


        private void HandleResponse(HttpResponseMessage response,  Link link) {

                // Start Loading content
                Task<HypermediaContent> loadContentTask = null;
                var content = response.Content;
                if (content != null) {
                     loadContentTask = new TaskFactory<HypermediaContent>()
                        .StartNew(() => new HypermediaContent(_SemanticsRegistry, response.Content, response.Headers));
               }

                // Determine actions that are mapped to this response
                var actions = from ra in _ResponseActions where ra.ShouldRespond(response) select ra;

                foreach (var responseHandler in actions) {
                    responseHandler.HandleResponseStart(link, response);
                }


                // Update RESTAgent State
                _LinkHistory.AddToHistory(link);

                _CurrentStatusCode = response.StatusCode;
                if (loadContentTask != null) {
                    loadContentTask.Wait();
                    if (loadContentTask.IsFaulted) throw new InvalidOperationException("Could not load content." + loadContentTask.Exception.Message);
                    _CurrentContent = loadContentTask.Result;
                } else {
                    _CurrentContent = null;
                }

                // Respond to content
                foreach (var responseHandler in actions)
                {
                    responseHandler.HandleResponseComplete(_CurrentContent);
                }

        }

        private void CreateDefaultResponseActions() {
            _ResponseActions.Add(new RedirectAction(this));
        }


        private Task<HttpContent> GetContentAsync(Link link)
        {
            if (link == null || link.Target == null) return null;
            link.MakeAbsolute(CurrentLocation);

            var content = _SemanticsRegistry.GetPreregisteredContent(link);
            if (content != null)
            {
                return Task.Factory.StartNew(() => content);
            }

            return _HttpClient.GetAsync(link.Target)
                .ContinueWith(task =>
                {
                    var response = task.Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return response.Content;
                    }
                    else
                    {
                        Debug.WriteLine("Error retrieving content from link with URI: " + link.Target + "  Status: " + response.StatusCode);
                        return null;
                    }
                }
            );
        }



        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient(new RestAgentHandler());
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue("HttpService", "1.0")));
            return httpClient;
        }


    
    }

    internal class RestAgentHandler : WebRequestHandler
    {


        public RestAgentHandler()
        {
            AllowAutoRedirect = false;
            CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);
        }



    }

    }