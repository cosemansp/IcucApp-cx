using System;
using IcucApp.Core.Configuration;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Presentation.ViewModels;

namespace IcucApp.Core.Presentation
{
    public interface IHomeView : IView
    {
        void DataBind(HomeViewModel model);
    }

    public class HomePresenter : IPresenter
    {
        private readonly IHomeView _view;
        private readonly ILog _log = LogManager.GetLogger(typeof(HomePresenter).Name);

        public HomePresenter(IHomeView view) 
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
            var model = new HomeViewModel();
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}