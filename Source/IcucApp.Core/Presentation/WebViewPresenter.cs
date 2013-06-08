using IcucApp.Core.Diagnostics;
using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Configuration;

namespace IcucApp.Presentation
{
    public interface IWebViewView : IView
    {
        void DataBind(WebViewModel model);
    }

    public class WebViewContext {
        public string Title { get; set; }
        public string Url { get; set;}
    }

    public class WebViewPresenter : IPresenter
    {
        private readonly IWebViewView _view;
        private WebViewContext _context;

        private readonly ILog _log = LogManager.GetLogger(typeof(WebViewPresenter).Name);

        public WebViewPresenter(IWebViewView view) 
        {
            _view = view;
        }

        public void Initialize(WebViewContext context)
        {
            _context = context;
        }

        public void OnViewShown()
        {
            DataBindView();
        }

        private void DataBindView()
        {
            var model = new WebViewModel
            { 
                Title = _context.Title,
                Url = _context.Url
            };
            _view.DataBind(model);
        }

        public void OnViewUnloaded()
        {
        }
    }
}