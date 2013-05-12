using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Core;
using IcucApp.Services.Syndication;
using System;
using System.Collections.Generic;

namespace IcucApp.Presentation
{
    public interface ILineupView : IView
    {
        void DataBind(LineupViewModel model);
    }

    public class LineupPresenter : IPresenter
    {
        private readonly ILineupView _view;
		private readonly IWordpressFeedAgent _websiteAgent;
		private readonly IDispatcher _dispatcher;
		private readonly IMapper<WordpressEntry, FeedData> _wordpressMapper;
        private readonly ILog _log = LogManager.GetLogger(typeof(LineupPresenter).Name);

        public LineupPresenter(ILineupView view,
		                       IDispatcher dispatcher,
		                       IWordpressFeedAgent websiteAgent,
		                       IMapper<WordpressEntry, FeedData> wordpressMapper) 
        {
            _view = view;
			_dispatcher = dispatcher;
			_websiteAgent = websiteAgent;
			_wordpressMapper = wordpressMapper;
        }

        public void Initialize()
        {
        }

        public void OnViewShown()
        {
			var data = CacheStore.Get<WordpressMessage>("lineupFeeds");
			if (data == null)
			{
				_dispatcher.ExecuteAsync(LoadWebsiteData, FinishedWebsiteLoading);
				_view.DataBind(LineupViewModel.Loading);
				return;
			}
			DataBindView(data.Entries);
        }

		WordpressMessage LoadWebsiteData() {
			return _websiteAgent.GetFeeds("2013/category/app-lineup");
		}
		
		private void FinishedWebsiteLoading(WordpressMessage result, Exception exception)
		{
			if (exception != null) {
				_log.ErrorFormat("failed to load data: {0}", exception.Message);
				return;
			}
			
			CacheStore.Set("lineupFeeds", result);
			DataBindView(result.Entries);
		}

		private void DataBindView(IEnumerable<WordpressEntry> entries)
		{
			try {
				var model = new LineupViewModel();
				if (entries != null) 
				{
					foreach (var websiteEntry in entries)
					{
						model.Entries.Add(_wordpressMapper.Map(websiteEntry));
					}
				}
				_view.DataBind(model);
			}
			catch(Exception ex) {
				_log.ErrorFormat("failed to bind data: {0}", ex.Message);
				_view.DataBind(LineupViewModel.Empty);
			}
		}

        public void OnViewUnloaded()
        {
        }

		public void Refresh ()
		{

		}
    }
}