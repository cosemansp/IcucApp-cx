using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Configuration;

namespace IcucApp.Presentation
{
    public interface ITicketView : IView
    {
        void DataBind(TicketModel model);
    }

    public class TicketPresenter : IPresenter
    {
        private readonly ITicketView _view;
		private readonly AppSettings _appSetting;

        private readonly ILog _log = LogManager.GetLogger(typeof(TicketPresenter).Name);

        public TicketPresenter(ITicketView view, AppSettings appSettings) 
        {
            _view = view;
			_appSetting = appSettings;
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
            var model = new TicketModel();
			model.Url = _appSetting.TicketUrl;
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}