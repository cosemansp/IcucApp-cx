using IcucApp.Core.Presentation;
using IcucApp.Core.Presentation.ViewModels;
using IcucApp.Core.Touch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace IcucApp.Touch.ViewControllers
{
    public class Tab2ViewController : MvpDialogViewController<Tab2Presenter>, ITab2View
    {
        public Tab2ViewController()
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

        public void DataBind(Tab2ViewModel model)
        {
            var rootElement = new RootElement("Tab2");
            Root = rootElement;
        }

    }
}