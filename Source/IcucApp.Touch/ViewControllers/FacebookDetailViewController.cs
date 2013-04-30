
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;

namespace IcucApp.ViewControllers
{
    public class FacebookDetailViewController : MvpViewController<TicketPresenter>, ITicketView
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

        public void DataBind(TicketModel model)
        {
        }
    }
}