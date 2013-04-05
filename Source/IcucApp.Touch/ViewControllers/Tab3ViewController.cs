using IcucApp.Core.Presentation;
using IcucApp.Core.Presentation.ViewModels;
using IcucApp.Core.Touch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace IcucApp.Touch.ViewControllers
{
    public class Tab3ViewController : MvpDialogViewController<Tab3Presenter>, ITab3View
    {
        public Tab3ViewController()
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

        public void DataBind(Tab3ViewModel model)
        {
            var rootElement = new RootElement("Tab3");
            Root = rootElement;
        }

    }
}