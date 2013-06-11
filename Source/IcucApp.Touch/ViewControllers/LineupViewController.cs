using IcucApp.Core;
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using IcucApp.ViewControllers.Elements;
using BigTed;

namespace IcucApp.ViewControllers
{
    public class LineupViewController : MvpDialogViewController<LineupPresenter>, ILineupView
    {
        private UISegmentedControl _segmentedControl;

        public LineupViewController()
            : base(UITableViewStyle.Grouped)
        {
			EnableRefresh();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _segmentedControl = new UISegmentedControl(new object[] {"Main Room", "Fucia"});
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

        public void DataBind(LineupViewModel model)
        {
            EndRefreshing();

			var rootElement = new RootElement("Line-up");
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
				var element = new LineupEntryElement(entry);
				section.Add(element);
			}
			
			rootElement.Add(section);
			Root = rootElement;
			
            ShowSpinner(model.IsLoading);
        }

		protected override void OnPullDownRefresh (object sender, System.EventArgs e)
		{
            Presenter.Refresh();
		}

        private void Retry(LoadMoreElement element) {
            Presenter.Refresh();
        }

    }
}