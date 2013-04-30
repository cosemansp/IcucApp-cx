using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace IcucApp.ViewControllers
{
    public class LineupViewController : MvpDialogViewController<LineupPresenter>, ILineupView
    {
        public LineupViewController()
            : base(UITableViewStyle.Plain)
        {
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
            var rootElement = new RootElement("Line up");
            Root = rootElement;
        }

    }
}