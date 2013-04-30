using System;
using System.Collections.Generic;
using IcucApp.Core;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Facebook;

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
        private readonly IFacebookFeedAgent _agent;
        private readonly INavigator _navigator;
        private readonly IMapper<FeedEntry, FeedData> _mapper;
        private readonly ILog _log = LogManager.GetLogger(typeof(NewsPresenter).Name);

        public NewsPresenter(INewsView view, 
                             IDispatcher dispatcher, 
                             IFacebookFeedAgent agent,
                             INavigator navigator,
                             IMapper<FeedEntry, FeedData> mapper )
        {
            _view = view;
            _dispatcher = dispatcher;
            _agent = agent;
            _navigator = navigator;
            _mapper = mapper;
        }

        public void Initialize()
        {
        }

        public void OnViewShown()
        {
            _dispatcher.ExecuteAsync(LoadData, FinishedLoading);

            DataBindView(null);
        }

        private void FinishedLoading(FeedsMessage result, Exception exception)
        {
			if (CacheStore.Exists<List<FeedEntry>>("facebookFeeds")) {
            	CacheStore.Remove<List<FeedEntry>>("facebookFeeds");
			}
            CacheStore.Add("facebookFeeds", result.entries);
            DataBindView(result.entries);
        }

        private FeedsMessage LoadData()
        {
            return _agent.GetFeeds("441615792534282" /* icuc feedId */);
        }

        private void DataBindView(IEnumerable<FeedEntry> entries)
        {
            var model = new NewsViewModel();
			if (entries != null) 
			{
	            foreach (var feedEntry in entries)
	            {
	                model.Entries.Add(_mapper.Map(feedEntry));
	            }
			}
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }

		public void OpenNewsFeed (int selectedSegment)
		{
			// throw new System.NotImplementedException ();
		}

        public void OnClickedFacebookFeed(string feedId)
        {
            _navigator.PushPresenter<FacebookDetailPresenter>(_view, feedId);
        }

        public void OnClickedWebsiteFeed(string feedId)
        {
            _navigator.PushPresenter<WebsiteDetailPresenter>(_view, feedId);
        }
    }
}