using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using IcucApp.Core;
using BigTed;
using System;
using IcucApp.ViewControllers.Elements;

namespace IcucApp.ViewControllers
{
	public class InfoViewController : MvpDialogViewController<InfoPresenter>, IInfoView
    {
		private UIWebView _webView;
        private bool _refresh = false;
		private int _currentContentHashcode;

		public InfoViewController()
			: base(UITableViewStyle.Grouped)
		{
			EnableRefresh();
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			// title
			Title = "Informatie";
			
			// Add WebView
			_webView = new UIWebView()
			{
				Frame = new RectangleF(3, 3, 300, 50), //View.Frame.Height - Display.TabBarHeight - Display.NavigationBarHeight),
				// BackgroundColor = UIColor.White
				Alpha = 0.0f
			};
			_webView.ShouldStartLoad = (webView, request, navType) =>
			{
				if (navType == UIWebViewNavigationType.LinkClicked)
				{
					// links
					Presenter.OnOpenLinkInBrowser(new Uri(request.Url.ToString()));
					return false;
				}
				return true;
			};
			
			_webView.LoadStarted += (sender, e) => {
			};
			_webView.LoadFinished += (sender, e) => {
                ShowSpinner(false);

				var rootElement = new RootElement("Informatie");
				var section = new Section();
				section.Add(new WebViewElement(_webView));
				rootElement.Add(section);
				Root = rootElement;
			};

			View.AddSubview(_webView);
			
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

        public void DataBind(InfoViewModel model)
        {
            ShowSpinner(model.IsLoading);
            if (!model.IsLoading)
                EndRefreshing();

            if (!model.ErrorMessage.IsNullOrEmpty())
            {
                var rootElement = new RootElement("Informatie");
                var section = new Section();
                var loadMore = new LoadingErrorElement();
                loadMore.Tapped += (object sender, System.EventArgs e) => {
                    Presenter.ReloadAll();
                    _refresh = true;
                };
                section.Add(loadMore);
                rootElement.Add(section);
                Root = rootElement;
                return;
            }

            if (!model.IsLoading) {
    			var template = new InfoTemplate () { Model = model };
    			var htmlContent = template.GenerateString();
    			if (_currentContentHashcode != htmlContent.GetHashCode() || _refresh) {
    				_webView.LoadHtmlString (htmlContent, null);
    				_currentContentHashcode = htmlContent.GetHashCode();
                    _refresh = false;
                }
            }
        }

		protected override void OnPullDownRefresh (object sender, System.EventArgs e)
		{
            Presenter.Reload();
            _refresh = true;
		}

    }
}