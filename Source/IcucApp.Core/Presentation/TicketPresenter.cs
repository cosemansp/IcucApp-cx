﻿using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;

namespace IcucApp.Presentation
{
    public interface ITicketView : IView
    {
        void DataBind(TicketModel model);
    }

    public class TicketPresenter : IPresenter
    {
        private readonly ITicketView _view;
        private readonly ILog _log = LogManager.GetLogger(typeof(TicketPresenter).Name);

        public TicketPresenter(ITicketView view) 
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
            var model = new TicketModel();
			model.Url = "https://m.ticketscript.com/channel/web2/start-order/rid/3JB82HDY/language/nl";
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}