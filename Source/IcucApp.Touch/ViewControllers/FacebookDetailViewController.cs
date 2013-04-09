using IcucApp.Core.Presentation;
using IcucApp.Core.Presentation.ViewModels;
using IcucApp.Core.Touch.UIKit;

namespace IcucApp.Touch.ViewControllers
{
    public class FacebookDetailViewController : MvpViewController<FacebookDetailPresenter>, IFacebookDetailView
    {
        public FacebookDetailViewController(string context)
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

        public void DataBind(FacebookDetailModel model)
        {
        }
    }
}