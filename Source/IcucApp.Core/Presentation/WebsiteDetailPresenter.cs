using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;

namespace IcucApp.Presentation
{
    public interface IWebsiteDetailView : IView
    {
        void DataBind(WebsiteDetailModel model);
    }

    public class WebsiteDetailPresenter : IPresenter
    {
        private IWebsiteDetailView _view;

        public WebsiteDetailPresenter(IWebsiteDetailView view)
        {
            _view = view;
        }
    }
}