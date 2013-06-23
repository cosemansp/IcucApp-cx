
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
	public class WebViewController : MvpViewController<WebViewPresenter>, IWebViewView
    {
		private UIWebView _webView;
		private string _currentUrl;
        private UIActivityIndicatorView _activitySpinner;
        private WebViewContext _context;

		public WebViewController(string context)
		{
            if (context == null)
                throw new System.ArgumentNullException("context");

            _context = context.FromJson<WebViewContext>();
		}

        public WebViewController(WebViewContext context)
        {
            if (context == null)
                throw new System.ArgumentNullException("context");

            _context = context;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			// title
            Title = _context.Title;

			// Add WebView
			_webView = new UIWebView()
			{
                Frame = new RectangleF(0, 0, 320, View.Frame.Height - Display.TabBarHeight - Display.NavigationBarHeight),
				BackgroundColor = UIColor.White,
				Alpha = 1.0f
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
                ShowSpinner();
			};

			_webView.LoadFinished += (sender, e) => {
                ShowSpinner(false);
			};

            // init presenter
            Presenter.Initialize(_context);
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			
			// navigationbar background
			NavigationController.NavigationBar.SetBackgroundImage(UIImage.FromBundle("navbar"), UIBarMetrics.Default);
			
			// init presenter
			Presenter.OnViewShown();
		}

        public void DataBind(WebViewModel model)
        {
            ShowSpinner(false);
			//if (_currentUrl != model.Url) {
				var url = new NSUrl(model.Url);
				var request = new NSUrlRequest(url);

				_webView.LoadRequest(request);
				_currentUrl = model.Url;
			//}
        }

        protected void ShowSpinner(bool show = true)
        {
            if (!show) {
                if (_activitySpinner != null) {
                    _activitySpinner.Hidden = true;
                    _activitySpinner.RemoveFromSuperview();
                    _activitySpinner = null;
                }
                return;
            }

            if (_activitySpinner == null) {
                // derive the center x and y
                float centerX = View.Frame.Width / 2;
                float centerY = View.Frame.Height / 2;

                _activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
                _activitySpinner.Frame = new RectangleF(centerX - (_activitySpinner.Frame.Width / 2),
                                                        centerY - _activitySpinner.Frame.Height - 20,
                                                        _activitySpinner.Frame.Width,
                                                        _activitySpinner.Frame.Height);
                _activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
                View.AddSubview(_activitySpinner);
            }
            _activitySpinner.Hidden = false;
            _activitySpinner.StartAnimating();
        }
    }
}