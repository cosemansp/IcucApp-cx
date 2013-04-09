using System;
using System.Collections.Generic;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Presentation.ViewModels;
using IcucApp.Core.Services.Facebook;

namespace IcucApp.Core.Presentation
{
    public interface IHomeView : IView
    {
        void DataBind(HomeViewModel model);
    }

    public class HomePresenter : IPresenter
    {
        private readonly IHomeView _view;
        private readonly IDispatcher _dispatcher;
        private readonly IFacebookFeedAgent _agent;
        private readonly INavigator _navigator;
        private readonly IMapper<FeedEntry, FeedData> _mapper;
        private readonly ILog _log = LogManager.GetLogger(typeof(HomePresenter).Name);

        public HomePresenter(IHomeView view, 
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
            CacheStore.Remove<List<FeedEntry>>("facebookFeeds");
            CacheStore.Add("facebookFeeds", result.entries);
            DataBindView(result.entries);
        }

        private FeedsMessage LoadData()
        {
            return _agent.GetFeeds("441615792534282" /* icuc feedId */);
        }

        private void DataBindView(IEnumerable<FeedEntry> entries)
        {
            var model = new HomeViewModel();
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

        public void OnClickedFeed(string feedId)
        {
            _navigator.PushPresenter<FacebookDetailPresenter>(_view, feedId);
        }
    }
}