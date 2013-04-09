using IcucApp.Core.Diagnostics;
using IcucApp.Core.Presentation.ViewModels;

namespace IcucApp.Core.Presentation
{
    public interface IFacebookDetailView : IView
    {
        void DataBind(FacebookDetailModel model);
    }

    public class FacebookDetailPresenter : IPresenter
    {
        private readonly IFacebookDetailView _view;
        private readonly ILog _log = LogManager.GetLogger(typeof(FacebookDetailPresenter).Name);

        public FacebookDetailPresenter(IFacebookDetailView view) 
        {
            _view = view;
        }

        public void Initialize()
        {
        }

        public void OnViewShown()
        {
            DataBindView();
        }

        private void DataBindView()
        {
            var model = new FacebookDetailModel();
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}