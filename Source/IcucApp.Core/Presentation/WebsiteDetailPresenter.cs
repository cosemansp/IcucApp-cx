using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Core;
using IcucApp.Services.Syndication;
using System;

namespace IcucApp.Presentation
{
    public interface IWebsiteDetailView : IView
    {
        void DataBind(WebsiteDetailModel model);
    }

    public class WebsiteDetailPresenter : IPresenter
    {
        private IWebsiteDetailView _view;
        private WordpressEntry _currentEntry;
        private IMapper<WordpressEntry, FeedData> _mapper;
        private ICache _cache;
        private IWebBrowser _webBrowser;
        private INavigator _navigator;

        public WebsiteDetailPresenter(IWebsiteDetailView view, 
                                       ICache cache,
                                       IWebBrowser webBrowser,
                                       INavigator navigator,
                                       IMapper<WordpressEntry, FeedData> mapper) 
        {
            _view = view;
            _cache = cache;
            _mapper = mapper;
            _navigator = navigator;
            _webBrowser = webBrowser;
        }

        public void Initialize(string context)
        {
            var feed = _cache.GetWebsiteFeed();
            foreach (var item in feed.Data.Entries)
            {
                if (item.id == context)
                {
                    _currentEntry = item;
                }
            }
        }

        public void OnViewShown()
        {
            var data = _mapper.Map(_currentEntry);
            _view.DataBind(new WebsiteDetailModel(data));
        }

        public void OnOpenLinkInView(Uri uri, string title)
        {
            var context = new WebViewContext
            {
                Title = title,
                Url = uri.ToString()
            };
            _navigator.PushPresenter<WebViewPresenter>(_view, context);
        }

        public void OnOpenLinkInBrowser(Uri uri)
        {
            _webBrowser.OpenUrl(uri.ToString());
        }
    }
}