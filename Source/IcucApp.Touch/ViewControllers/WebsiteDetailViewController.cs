using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Presentation.ViewModels;

namespace IcucApp.ViewControllers
{
    public class WebsiteDetailViewController : MvpViewController<TicketPresenter>, ITicketView
    {
        public WebsiteDetailViewController(string context)
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