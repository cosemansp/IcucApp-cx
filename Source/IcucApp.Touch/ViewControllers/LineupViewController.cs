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
            : base(UITableViewStyle.Plain)
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
				var element = new LineupEntryElement(entry.Title);
				section.Add(element);
			}
			
			rootElement.Add(section);
			Root = rootElement;
			
			if (model.IsLoading) {
				BTProgressHUD.Show("Loading...");
			}
			else {
				BTProgressHUD.Dismiss();
			}
        }

    }
}