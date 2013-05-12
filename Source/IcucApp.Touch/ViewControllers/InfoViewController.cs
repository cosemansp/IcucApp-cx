using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using IcucApp.Core;
using BigTed;
using System;

namespace IcucApp.ViewControllers
{
    public class InfoViewController : MvpViewController<InfoPresenter>, IInfoView
    {
		private UIWebView _webView;
		private int _currentContentHashcode;

        public InfoViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			// title
			Title = "Informatie";
			
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
					// links
					Presenter.OnOpenLinkInBrowser(new Uri(request.Url.ToString()));
					return false;
				}
				View.AddSubview(_webView);
				return true;
			};
			
			_webView.LoadStarted += (sender, e) => {
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

        public void DataBind(InfoViewModel model)
        {
			if (model.IsLoading) {
				BTProgressHUD.Show("Informatie wordt geladen...");
			}

			var template = new InfoTemplate () { Model = model };
			var htmlContent = template.GenerateString();
			if (_currentContentHashcode != htmlContent.GetHashCode()) {
				_webView.LoadHtmlString (htmlContent, null);
				_currentContentHashcode = htmlContent.GetHashCode();
			}
        }

    }
}