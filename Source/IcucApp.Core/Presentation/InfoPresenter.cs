using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;

namespace IcucApp.Presentation
{
    public interface IInfoView : IView
    {
        void DataBind(InfoViewModel model);
    }

    public class InfoPresenter : IPresenter
    {
        private readonly IInfoView _view;
        private readonly ILog _log = LogManager.GetLogger(typeof(InfoPresenter).Name);

        public InfoPresenter(IInfoView view) 
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
            var model = new InfoViewModel();
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}