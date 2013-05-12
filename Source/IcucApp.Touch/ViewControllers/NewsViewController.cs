using IcucApp.Core;
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using IcucApp.ViewControllers.Elements;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using BigTed;

namespace IcucApp.ViewControllers
{
    public class NewsViewController : MvpDialogViewController<NewsPresenter>, INewsView
    {
		UISegmentedControl _segmentedControl;

        public NewsViewController()
            : base(UITableViewStyle.Grouped)
        {
			EnableRefresh();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			_segmentedControl = new UISegmentedControl(new object[] {"Facebook", "Website"});
			_segmentedControl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			_segmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			// _segmentedControl.TintColor = UIColor.Gray;
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

            // init presenter
            Presenter.OnViewShown();
        }

        public void DataBind(NewsViewModel model)
        {
            var rootElement = new RootElement("ICUC");
            var section = new Section();
            foreach (var entry in model.Entries)
            {
                if (entry.Title.Trim(" ".ToCharArray()).IsNullOrEmpty())
                    continue;
				if (entry.Type == "facebook") {
	                var element = new FacebookEntryElement(entry);
					element.Tapped += (sender, e) => Presenter.OnClickedFacebookFeed(entry.Id);
	                section.Add(element);
				}
				else {
					var element = new WebsiteEntryElement(entry);
					element.Tapped += (sender, e) => Presenter.OnClickedWebsiteFeed(entry.Id);
					section.Add(element);
				}
            }

            rootElement.Add(section);
            Root = rootElement;

			if (model.IsLoading) {
				BTProgressHUD.Show("Het nieuws wordt geladen...");
			}
			else {
				BTProgressHUD.Dismiss();
			}
        }
    }
}