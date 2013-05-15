
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.UIKit;
using System.Drawing;
using IcucApp.Core;
using MonoTouch.Foundation;
using BigTed;
using MonoTouch.Dialog;
using IcucApp.ViewControllers.Elements;

namespace IcucApp.ViewControllers
{
	public class TicketViewController : MvpDialogViewController<TicketPresenter>, ITicketView
    {
		private UIWebView _webView;
		private string _currentUrl;

		public TicketViewController()
			: base(UITableViewStyle.Grouped)
		{
			EnableRefresh();
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			// title
			Title = "Ticket";

			// Add WebView
			_webView = new UIWebView()
			{
				Frame = new RectangleF(0, 0, 300, View.Frame.Height - Display.TabBarHeight - Display.NavigationBarHeight),
				// BackgroundColor = UIColor.White,
				Alpha = 0.0f
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
				return true;
			};

			View.AddSubview(_webView);

			_webView.LoadStarted += (sender, e) => {
				BTProgressHUD.Show("Ticketshop wordt geladen...");
			};

			_webView.LoadFinished += (sender, e) => {
				BTProgressHUD.Dismiss();

				var rootElement = new RootElement("Ticket");
				var section = new Section();
				section.Add(new WebViewElement(_webView));
				rootElement.Add(section);
				Root = rootElement;
			};

            // init presenter
            Presenter.Initialize();
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			
			// navigationbar background
			NavigationController.NavigationBar.SetBackgroundImage(UIImage.FromBundle("navbar"), UIBarMetrics.Default);
			
			// table background
			var backgroundView = new UIImageView(UIImage.FromBundle("background"));
			this.TableView.BackgroundView = backgroundView;
			
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

		protected override void OnPullDownRefresh (object sender, System.EventArgs e)
		{
			EndRefreshing();
		}
    }
}