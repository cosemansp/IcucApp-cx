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
        public LineupViewController()
            : base(UITableViewStyle.Grouped)
        {
			EnableRefresh();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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
			var rootElement = new RootElement("Line-up");
			var section = new Section();
			foreach (var entry in model.Entries)
			{
				if (entry.Title.Trim(" ".ToCharArray()).IsNullOrEmpty())
					continue;
				var element = new LineupEntryElement(entry);
				section.Add(element);
			}
			
			rootElement.Add(section);
			Root = rootElement;
			
			if (model.IsLoading) {
				BTProgressHUD.Show("Line-up wordt geladen...");
			}
			else {
				BTProgressHUD.Dismiss();
			}
        }

		protected override void OnPullDownRefresh (object sender, System.EventArgs e)
		{
			EndRefreshing();
		}

    }
}