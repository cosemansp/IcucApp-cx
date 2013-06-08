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
        private readonly ICache _cache;
        private readonly IDataLoader _dataLoader;
		private readonly IMapper<WordpressEntry, FeedData> _wordpressMapper;
        private int _segment = 0;
        private bool _isActive;
        private readonly ILog _log = LogManager.GetLogger(typeof(LineupPresenter).Name);

        public LineupPresenter(ILineupView view,
		                       ICache cache,
		                       IDataLoader dataLoader,
		                       IMapper<WordpressEntry, FeedData> wordpressMapper) 
        {
            _view = view;
			_cache = cache;
			_dataLoader = dataLoader;
			_wordpressMapper = wordpressMapper;
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
            var feed = _cache.GetLineupFeed(_segment == 0 ? "lineup" : "lineup2");
			if (feed == null)
			{
                // no data availble, is loading
				_view.DataBind(LineupViewModel.Loading);
				return;
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
                    model.ErrorMessage = "Ophalen icue nieuws is mislukt";
                }
                if (context.Data != null && context.Data.Entries != null) 
				{
                    foreach (var websiteEntry in context.Data.Entries)
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
            _dataLoader.ReloadWebsiteLineup();
		}
    }
}