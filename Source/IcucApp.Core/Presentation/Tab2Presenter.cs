using IcucApp.Core.Diagnostics;
using IcucApp.Core.Presentation.ViewModels;

namespace IcucApp.Core.Presentation
{
    public interface ITab2View : IView
    {
        void DataBind(Tab2ViewModel model);
    }

    public class Tab2Presenter : IPresenter
    {
        private readonly ITab2View _view;
        private readonly ILog _log = LogManager.GetLogger(typeof(Tab2Presenter).Name);

        public Tab2Presenter(ITab2View view) 
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
            var model = new Tab2ViewModel();
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}