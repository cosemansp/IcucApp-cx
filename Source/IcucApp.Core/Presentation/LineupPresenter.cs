using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;

namespace IcucApp.Presentation
{
    public interface ILineupView : IView
    {
        void DataBind(LineupViewModel model);
    }

    public class LineupPresenter : IPresenter
    {
        private readonly ILineupView _view;
        private readonly ILog _log = LogManager.GetLogger(typeof(LineupPresenter).Name);

        public LineupPresenter(ILineupView view) 
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
            var model = new LineupViewModel();
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}