using System;
using System.Collections.Generic;
using IcucApp.Core;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Facebook;
using IcucApp.Services.Syndication;

namespace IcucApp.Presentation
{
    public interface INewsView : IView
    {
        void DataBind(NewsViewModel model);
    }

    public class NewsPresenter : IPresenter
    {
        private readonly INewsView _view;
        private readonly IDispatcher _dispatcher;
        private readonly IFacebookFeedAgent _facebookAgent;
		private readonly IWordpressFeedAgent _websiteAgent;
        private readonly INavigator _navigator;
        private readonly IMapper<FacebookEntry, FeedData> _facebookMapper;
		private readonly IMapper<WordpressEntry, FeedData> _wordpressMapper;

        private readonly ILog _log = LogManager.GetLogger(typeof(NewsPresenter).Name);
		int _segment;

        public NewsPresenter(INewsView view, 
                             IDispatcher dispatcher, 
                             IFacebookFeedAgent facebookAgent,
		                     IWordpressFeedAgent websiteAgent,
                             INavigator navigator,
                             IMapper<FacebookEntry, FeedData> facebookMapper,
		                     IMapper<WordpressEntry, FeedData> wordpressMapper)
        {
			_websiteAgent = websiteAgent;
            _view = view;
            _dispatcher = dispatcher;
            _facebookAgent = facebookAgent;
            _navigator = navigator;
			_facebookMapper = facebookMapper;
			_wordpressMapper = wordpressMapper;
			_segment = 0;
        }

        public void Initialize()
        {
        }

        public void OnViewShown()
        {
			if (_segment == 0) {
				LoadAndBindFacebookFeed();
			}
			if (_segment == 1) {
				LoadAndBindWebsiteNews();
			}
        }

        private void FinishedFacebookLoading(FacebookMessage result, Exception exception)
        {
			if (exception != null) {
				_log.ErrorFormat("failed to load data: {0}", exception.Message);
				_view.DataBind(NewsViewModel.Empty);
				return;
			}

            CacheStore.Set("facebookFeeds", result);
            DataBindView(result.entries);
        }

        private FacebookMessage LoadFacebookData()
        {
            return _facebookAgent.GetFeeds("441615792534282" /* icuc feedId */);
        }

        private void DataBindView(IEnumerable<FacebookEntry> entries)
        {
            var model = new NewsViewModel();
			if (entries != null) 
			{
	            foreach (var feedEntry in entries)
	            {
					var dataEntry = _facebookMapper.Map(feedEntry);
					dataEntry.ImageUrl = new Uri("http://graph.facebook.com/{0}/picture".FormatWith("441615792534282"));
					model.Entries.Add(dataEntry);
	            }
			}
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }

		public void OpenNewsFeed (int selectedSegment)
		{
			if (selectedSegment == 0) {
				LoadAndBindFacebookFeed();
			}

			if (selectedSegment == 1) {
				LoadAndBindWebsiteNews();
			}

			_segment = selectedSegment;
		}

        public void OnClickedFacebookFeed(string feedId)
        {
            _navigator.PushPresenter<FacebookDetailPresenter>(_view, feedId);
        }

        public void OnClickedWebsiteFeed(string feedId)
        {
            _navigator.PushPresenter<WebsiteDetailPresenter>(_view, feedId);
        }

		public void Refresh ()
		{
			throw new System.NotImplementedException ();
		}

		void LoadAndBindFacebookFeed ()
		{
			var data = CacheStore.Get<FacebookMessage>("facebookFeeds");
			if (data == null)
			{
				_dispatcher.ExecuteAsync(LoadFacebookData, FinishedFacebookLoading);
				_view.DataBind(NewsViewModel.Loading);
				return;
			}
			DataBindView(data.entries);
		}

		void LoadAndBindWebsiteNews ()
		{
			var data = CacheStore.Get<WordpressMessage>("websiteFeeds");
			if (data == null)
			{
				_dispatcher.ExecuteAsync(LoadWebsiteData, FinishedWebsiteLoading);
				_view.DataBind(NewsViewModel.Loading);
				return;
			}
			DataBindView(data.Entries);
		}

		WordpressMessage LoadWebsiteData() {
			return _websiteAgent.GetFeeds("2013/category/app-news");
		}

		private void FinishedWebsiteLoading(WordpressMessage result, Exception exception)
		{
			if (exception != null) {
				_log.ErrorFormat("failed to load data: {0}", exception.Message);
				_view.DataBind(NewsViewModel.Empty);
				return;
			}
			
			CacheStore.Set("websiteFeeds", result);
			DataBindView(result.Entries);
		}

		private void DataBindView(IEnumerable<WordpressEntry> entries)
		{
			var model = new NewsViewModel();
			if (entries != null) 
			{
				foreach (var websiteEntry in entries)
				{
					model.Entries.Add(_wordpressMapper.Map(websiteEntry));
				}
			}
			_view.DataBind(model);
		}

    }
}