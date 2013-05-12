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
		private readonly IDispatcher _dispatcher;
		private readonly IWordpressFeedAgent _websiteAgent;
		private readonly IWebBrowser _webBrowser;

        private readonly ILog _log = LogManager.GetLogger(typeof(InfoPresenter).Name);

        public InfoPresenter(IInfoView view,
		                     IDispatcher dispatcher,
		                     IWordpressFeedAgent websiteAgent,
		                     IWebBrowser webBrowser) 
        {
            _view = view;
			_dispatcher = dispatcher;
			_websiteAgent = websiteAgent;
			_webBrowser = webBrowser;
        }

        public void Initialize()
        {
        }

        public void OnViewShown()
        {
			var data = CacheStore.Get<WordpressEntry>("infoFeed");
			if (data == null)
			{
				_dispatcher.ExecuteAsync(LoadData, FinishedLoading);
				_view.DataBind(InfoViewModel.Loading);
				return;
			}

			DataBindView(data);
        }

		public void OnOpenLinkInBrowser (Uri uri)
		{
			_webBrowser.OpenUrl(uri.ToString());
		}

		private WordpressEntry LoadData()
		{
			return _websiteAgent.GetSingleFeed("2013/app-info");
		}

		private void FinishedLoading(WordpressEntry result, Exception exception)
		{
			if (exception != null) {
				_log.ErrorFormat("failed to load data: {0}", exception.Message);
				return;
			}
			
			CacheStore.Set("infoFeed", result);
			DataBindView(result);
		}

        private void DataBindView(WordpressEntry entry)
        {
            var model = new InfoViewModel();
			model.Title = entry.title;
			model.Content = entry.content;
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}