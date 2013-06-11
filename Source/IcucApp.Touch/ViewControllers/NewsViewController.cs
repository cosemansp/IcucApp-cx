using IcucApp.Core;
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using IcucApp.ViewControllers.Elements;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using BigTed;
using IcucApp.Core.Touch.UIKit;
using System.Drawing;

namespace IcucApp.ViewControllers
{
    public class NewsViewController : MvpDialogViewController<NewsPresenter>, INewsView
    {
        private UISegmentedControl _segmentedControl;

        public NewsViewController()
            : base(UITableViewStyle.Grouped)
        {
			EnableRefresh();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			_segmentedControl = new UISegmentedControl(new object[] {"Facebook", "Algemeen"});
			_segmentedControl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			_segmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			_segmentedControl.TintColor = UIColor.DarkGray;
			_segmentedControl.Frame = new System.Drawing.RectangleF(0, 0, 200, 30);
			_segmentedControl.SelectedSegment = 0;
			_segmentedControl.ValueChanged += (object sender, System.EventArgs e) => {
				Presenter.OpenNewsFeed(_segmentedControl.SelectedSegment);
			};
			this.NavigationItem.TitleView = _segmentedControl;


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

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public void DataBind(NewsViewModel model)
        {
            EndRefreshing();

            var rootElement = new RootElement("Nieuws");
            var section = new Section();

            if (!model.ErrorMessage.IsNullOrEmpty())
            {
                var loadMore = new LoadingErrorElement();
                loadMore.Tapped += (object sender, System.EventArgs e) => {
                    Presenter.ReloadAll();
                };
                section.Add(loadMore);
            }
            foreach (var entry in model.Entries)
            {
                if (entry.Title.Trim(" ".ToCharArray()).IsNullOrEmpty())
                    continue;
				if (entry.Type == "facebook") {
	                var element = new FacebookEntryElement(entry);
                    element.Tapped += (sender, e) => {
                        var thisElement = sender as FacebookEntryElement;
                        Presenter.OnClickedFacebookFeed(thisElement.Data.Id);
                    };
	                section.Add(element);
				}
				else {
					var element = new WebsiteEntryElement(entry);
                    element.Tapped += (sender, e) => {
                        var thisElement = sender as WebsiteEntryElement;
                        Presenter.OnClickedWebsiteFeed(thisElement.Data.Id);
                    };
					section.Add(element);
				}
            }

            rootElement.Add(section);
            Root = rootElement;
            ShowSpinner(model.IsLoading);
        }

		protected override void OnPullDownRefresh (object sender, System.EventArgs e)
		{
            Presenter.Reload();
		}

        private void Retry(LoadMoreElement element) {
            Presenter.Reload();
        }

    }
}