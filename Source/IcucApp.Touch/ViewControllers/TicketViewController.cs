
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.UIKit;
using System.Drawing;
using IcucApp.Core;
using MonoTouch.Foundation;
using BigTed;

namespace IcucApp.ViewControllers
{
    public class TicketViewController : MvpViewController<TicketPresenter>, ITicketView
    {
		private UIWebView _webView;
		private string _currentUrl;

        public TicketViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			// title
			Title = "Ticket";

			// Add WebView
			_webView = new UIWebView()
			{
				Frame = new RectangleF(0, 0, 320, View.Frame.Height - Display.TabBarHeight - Display.NavigationBarHeight),
				BackgroundColor = UIColor.White
			};
			_webView.ShouldStartLoad = (webView, request, navType) =>
			{
				if (navType == UIWebViewNavigationType.LinkClicked)
				{
//					if (request.Url.Query != null) {
//						if (request.Url.Query.Contains("comments"))
//						{
//							// comments/reactions clicked
//							Presenter.ShowComments(_articleId);
//							return false;
//						}
//						if (request.Url.Query.Contains("banner") && !_adTarget.IsNullOrEmpty())
//						{
//							// ad clicked
//							Presenter.OnAdClicked(_adTarget);
//							return false;
//						}
//					}
//					// all other links
//					Presenter.OnOpenLinkInBrowser(new Uri(request.Url.ToString()));
//					return false;
				}
				View.AddSubview(_webView);
				return true;
			};

			_webView.LoadStarted += (sender, e) => {
				BTProgressHUD.Show("Ticketshop wordt geladen...");
			};

			_webView.LoadFinished += (sender, e) => {
				BTProgressHUD.Dismiss();
			};

            // init presenter
            Presenter.Initialize();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // init presenter
            Presenter.OnViewShown();
        }

        public void DataBind(TicketModel model)
        {
			if (_currentUrl != model.Url) {
				var url = new NSUrl(model.Url);
				var request = new NSUrlRequest(url);

				_webView.LoadRequest(request);
				_currentUrl = model.Url;
			}
        }
    }
}