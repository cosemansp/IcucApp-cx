using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;

namespace IcucApp.Presentation
{
    public interface IFacebookDetailView : IView
    {
        void DataBind(FacebookDetailModel model);
    }

    public class FacebookDetailPresenter : IPresenter
    {
        private IFacebookDetailView _view;

        public FacebookDetailPresenter(IFacebookDetailView view) 
        {
            _view = view;
        }
    }

   
}