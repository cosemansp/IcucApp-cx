using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Syndication;
using IcucApp.Services.Facebook;
using IcucApp.Core;
using System;

namespace IcucApp.Presentation
{
    public interface IFacebookDetailView : IView
    {
        void DataBind(FacebookDetailModel model);
    }

    public class FacebookDetailPresenter : IPresenter
    {
        private IFacebookDetailView _view;
        private FacebookEntry _currentEntry;
        private IMapper<FacebookEntry, FeedData> _mapper;
        private ICache _cache;
        private IWebBrowser _webBrowser;
        private INavigator _navigator;

        public FacebookDetailPresenter(IFacebookDetailView view, 
                                       ICache cache,
                                       IWebBrowser webBrowser,
                                       INavigator navigator,
                                       IMapper<FacebookEntry, FeedData> facebookMapper) 
        {
            _navigator = navigator;
            _view = view;
            _cache = cache;
            _mapper = facebookMapper;
            _webBrowser = webBrowser;
        }

        public void Initialize(string context)
        {
            var feed = _cache.GetFacebookFeed();
            foreach (var item in feed.Data.entries)
            {
                if (item.id == context)
                {
                    _currentEntry = item;
                }
            }
        }

        public void OnOpenLinkInBrowser(Uri uri)
        {
            _webBrowser.OpenUrl(uri.ToString());
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

        public void OnViewShown()
        {
            var data = _mapper.Map(_currentEntry);
            _view.DataBind(new FacebookDetailModel(data));
        }
    }

   
}