using IcucApp.Core;
using IcucApp.Core.Presentation;
using IcucApp.Core.Presentation.ViewModels;
using IcucApp.Core.Touch.UIKit;
using IcucApp.Touch.ViewControllers.Elements;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace IcucApp.Touch.ViewControllers
{
    public class HomeViewController : MvpDialogViewController<HomePresenter>, IHomeView
    {
        public HomeViewController()
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

        public void DataBind(HomeViewModel model)
        {
            var rootElement = new RootElement("ICUC");
            var section = new Section();
            foreach (var entry in model.Entries)
            {
                if (entry.Title.Trim(" ".ToCharArray()).IsNullOrEmpty())
                    continue;
                var element = new FacebookEntryElement(entry.Title);
                element.Tapped += () => Presenter.OnClickedFeed("");
                section.Add(element);
            }

            rootElement.Add(section);
            Root = rootElement;
        }
    }
}