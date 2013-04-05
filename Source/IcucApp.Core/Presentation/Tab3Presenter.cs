using IcucApp.Core.Diagnostics;
using IcucApp.Core.Presentation.ViewModels;

namespace IcucApp.Core.Presentation
{
    public interface ITab3View : IView
    {
        void DataBind(Tab3ViewModel model);
    }

    public class Tab3Presenter : IPresenter
    {
        private readonly ITab3View _view;
        private readonly ILog _log = LogManager.GetLogger(typeof(Tab3Presenter).Name);

        public Tab3Presenter(ITab3View view) 
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
            var model = new Tab3ViewModel();
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}