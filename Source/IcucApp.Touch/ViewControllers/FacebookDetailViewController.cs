
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.UIKit;
using System.Drawing;
using IcucApp.Core;
using MonoTouch.Dialog;
using IcucApp.ViewControllers.Elements;
using System;

namespace IcucApp.ViewControllers
{
    public class FacebookDetailViewController :  MvpDialogViewController<FacebookDetailPresenter>, IFacebookDetailView
    {
        private UIWebView _webView;
        private int _currentContentHashcode;
        private string _context;

        public FacebookDetailViewController(string context) : base(UITableViewStyle.Grouped)
        {
            _context = context;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Add WebView
            _webView = new UIWebView()
            {
                Frame = new RectangleF(0, 0, 300, 50), // View.Frame.Height - Display.TabBarHeight - Display.NavigationBarHeight),
                // BackgroundColor = UIColor.White
                Alpha = 0.0f
            };
            _webView.LoadFinished += (sender, e) => {
                ShowSpinner(false);

                var rootElement = new RootElement("Artikel");
                var section = new Section();
                section.Add(new WebViewElement(_webView));
                rootElement.Add(section);
                Root = rootElement;
            };
            _webView.ShouldStartLoad = (webView, request, navType) =>
            {
                if (navType == UIWebViewNavigationType.LinkClicked)
                {
                    //                  if (request.Url.Query != null) {
                    //                      if (request.Url.Query.Contains("comments"))
                    //                      {
                    //                          // comments/reactions clicked
                    //                          Presenter.ShowComments(_articleId);
                    //                          return false;
                    //                      }
                    //                      if (request.Url.Query.Contains("banner") && !_adTarget.IsNullOrEmpty())
                    //                      {
                    //                          // ad clicked
                    //                          Presenter.OnAdClicked(_adTarget);
                    //                          return false;
                    //                      }
                    //                  }
                    if (request.Url.AbsoluteString.Contains("youtube"))
                    {
                        Presenter.OnOpenLinkInView(new Uri(request.Url.ToString()), "Youtube");
                        return false;
                    }

                    // all other links
                    //Presenter.OnOpenLinkInBrowser(new Uri(request.Url.ToString()));
                    //return false;
                }
                return true;
            };

            View.AddSubview(_webView);

            // init presenter
            Presenter.Initialize(_context);
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

        public void DataBind(FacebookDetailModel model)
        {
            var template = new FacebookDetailTemplate () { Model = model };
            var htmlContent = template.GenerateString();
            if (_currentContentHashcode != htmlContent.GetHashCode()) {
                _webView.LoadHtmlString (htmlContent, null);
                _currentContentHashcode = htmlContent.GetHashCode();
            }
        }
    }
}