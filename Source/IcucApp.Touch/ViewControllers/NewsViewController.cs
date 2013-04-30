using IcucApp.Core;
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using IcucApp.ViewControllers.Elements;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace IcucApp.ViewControllers
{
    public class NewsViewController : MvpDialogViewController<NewsPresenter>, INewsView
    {
		UISegmentedControl _segmentedControl;

        public NewsViewController()
            : base(UITableViewStyle.Plain)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			_segmentedControl = new UISegmentedControl(new object[] {"Facebook", "Website"});
			_segmentedControl.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			_segmentedControl.ControlStyle = UISegmentedControlStyle.Bar;
			_segmentedControl.TintColor = UIColor.Gray;
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
                var element = new FacebookEntryElement(entry.Title);
                element.Tapped += () => Presenter.OnClickedFacebookFeed("");
                section.Add(element);
            }

            rootElement.Add(section);
            Root = rootElement;
        }
    }
}