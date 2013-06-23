using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Syndication;
using IcucApp.Core;
using System;
using IcucApp.Configuration;

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
        private readonly AppSettings _appSettings;
        private bool _isActive;

        private readonly ILog _log = LogManager.GetLogger(typeof(InfoPresenter).Name);

        public InfoPresenter(IInfoView view,
		                     ICache cache,
		                     IDataLoader dataLoader,
                             AppSettings appSettings,
		                     IWebBrowser webBrowser) 
        {
			_dataLoader = dataLoader;
            _view = view; 
			_cache = cache;
            _appSettings = appSettings;
			_webBrowser = webBrowser;
        }

        public void Initialize()
        {
            _dataLoader.DataLoaded += (sender, e) => {
                if (!_isActive) 
                    return;
                var feed = _cache.GetInfoFeed();
                if (feed != null && e.Context == "info") {
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

            if (feed.TimeStamp < DateTime.Now - _appSettings.TimeoutContent)
            {
                _dataLoader.ReloadWebsiteInfo();
            }

			DataBindView(feed);
        }

		public void OnOpenLinkInBrowser (Uri uri)
		{
			_webBrowser.OpenUrl(uri.ToString());
        }

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

        public void Reload()
        {
            _dataLoader.ReloadWebsiteInfo();
            //_view.DataBind(InfoViewModel.Loading);
        }

        public void ReloadAll()
        {
            _dataLoader.ReloadAllFeed();
            _view.DataBind(InfoViewModel.Loading);
        }
    }
}