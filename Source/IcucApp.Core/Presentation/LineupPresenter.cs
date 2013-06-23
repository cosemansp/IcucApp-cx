using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Core;
using IcucApp.Services.Syndication;
using System;
using System.Linq;
using System.Collections.Generic;
using IcucApp.Configuration;

namespace IcucApp.Presentation
{
    public interface ILineupView : IView
    {
        void DataBind(LineupViewModel model);
    }

    public class LineupPresenter : IPresenter
    {
        private readonly ILineupView _view;
        private readonly ICache _cache;
        private readonly IDataLoader _dataLoader;
		private readonly IMapper<WordpressEntry, FeedData> _wordpressMapper;
        private readonly AppSettings _appSettings;
        private int _segment = 0;
        private bool _isActive;
        private readonly ILog _log = LogManager.GetLogger(typeof(LineupPresenter).Name);

        public LineupPresenter(ILineupView view,
		                       ICache cache,
		                       IDataLoader dataLoader,
                               AppSettings appSettings,
		                       IMapper<WordpressEntry, FeedData> wordpressMapper) 
        {
            _view = view;
			_cache = cache;
			_dataLoader = dataLoader;
			_wordpressMapper = wordpressMapper;
            _appSettings = appSettings;
        }

        public void Initialize()
        {
            _dataLoader.DataLoaded += (sender, e) => {
                if (!_isActive)
                    return;
                var feed = _cache.GetLineupFeed(_segment == 0 ? "lineup" : "lineup2");
                if (feed != null) {
                    DataBindView(feed);
                }
            };
        }

        public void OnViewShown()
        {
            _isActive = true;
            var lineupFeedCategory = _segment == 0 ? "lineup" : "lineup2";
            var feed = _cache.GetLineupFeed(lineupFeedCategory);
			if (feed == null)
			{
                // no data availble, is loading
				_view.DataBind(LineupViewModel.Loading);
				return;
			}
            if (feed.TimeStamp < DateTime.Now - _appSettings.TimeoutContent)
            {
                _dataLoader.ReloadWebsiteLineup(lineupFeedCategory);
            }
			DataBindView(feed);
        }

        public void OpenNewsFeed(int segment) {
            _segment = segment;
            OnViewShown();
        }

		private void DataBindView(RequestContext<WordpressMessage> context)
		{
			try {
				var model = new LineupViewModel();
                if (context.Exception != null)
                {
                    model.ErrorMessage = "Ophalen line-up is mislukt";
                }

                if (context.Data != null && context.Data.Entries != null) 
				{
                    var sortedEntries = context.Data.Entries.Select(x => x).OrderBy(x => x.date).ToList();
                    foreach (var websiteEntry in sortedEntries)
					{
						model.Entries.Add(_wordpressMapper.Map(websiteEntry));
					}
                    model.LastUpdate = context.TimeStamp;
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

        public void ReloadAll()
        {
            _dataLoader.ReloadAllFeed();
            _view.DataBind(LineupViewModel.Loading);
        }

		public void Refresh ()
		{
            var lineupFeedCategory = _segment == 0 ? "lineup" : "lineup2";
            _dataLoader.ReloadWebsiteLineup(lineupFeedCategory);
		}
    }
}