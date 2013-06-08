using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Syndication;
using IcucApp.Core;
using System;

namespace IcucApp.Presentation
{
    public interface IInfoView : IView
    {
        void DataBind(InfoViewModel model);
    }

    public class InfoPresenter : IPresenter
    {
        private readonly IInfoView _view;
		private readonly IWebBrowser _webBrowser;
		private readonly IDataLoader _dataLoader;
		private readonly ICache _cache;
        private bool _isActive;
        private readonly ILog _log = LogManager.GetLogger(typeof(InfoPresenter).Name);

        public InfoPresenter(IInfoView view,
		                     ICache cache,
		                     IDataLoader dataLoader,
		                     IWebBrowser webBrowser) 
        {
			_dataLoader = dataLoader;
            _view = view; 
			_cache = cache;
			_webBrowser = webBrowser;
        }

        public void Initialize()
        {
            _dataLoader.DataLoaded += (sender, e) => {
                if (!_isActive) 
                    return;
                var feed = _cache.GetInfoFeed();
                if (feed != null) {
                    DataBindView(feed);
                }
            };
        }

        public void OnViewShown()
        {
            _isActive = true;
			var feed = _cache.GetInfoFeed();
			if (feed == null)
			{
				_view.DataBind(InfoViewModel.Loading);
				return;
			}

			DataBindView(feed);
        }

		public void OnOpenLinkInBrowser (Uri uri)
		{
			_webBrowser.OpenUrl(uri.ToString());
		}

//		private WordpressEntry LoadData()
//		{
//			return _websiteAgent.GetSingleFeed("2013/app-info");
//		}
//
//		private void FinishedLoading(WordpressEntry result, Exception exception)
//		{
//			if (exception != null) {
//				_log.ErrorFormat("failed to load data: {0}", exception.Message);
//				return;
//			}
//			
//			CacheStore.Set("infoFeed", result);
//			DataBindView(result);
//		}

        private void DataBindView(RequestContext<WordpressEntry> context)
        {
            var model = new InfoViewModel();
            if (context.Exception != null)
            {
                model.ErrorMessage = "Ophalen icue nieuws is mislukt";
            }
            if (context.Data != null) {
                model.Title = context.Data.title;
                model.Content = context.Data.content;
            }
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}