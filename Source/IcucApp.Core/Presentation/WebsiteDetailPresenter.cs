using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Core;
using IcucApp.Services.Syndication;

namespace IcucApp.Presentation
{
    public interface IWebsiteDetailView : IView
    {
        void DataBind(WebsiteDetailModel model);
    }

    public class WebsiteDetailPresenter : IPresenter
    {
        private IWebsiteDetailView _view;
        private WordpressEntry _currentEntry;
        private IMapper<WordpressEntry, FeedData> _mapper;
        private ICache _cache;

        public WebsiteDetailPresenter(IWebsiteDetailView view, 
                                       ICache cache,
                                       IMapper<WordpressEntry, FeedData> mapper) 
        {
            _view = view;
            _cache = cache;
            _mapper = mapper;
        }

        public void Initialize(string context)
        {
            var feed = _cache.GetWebsiteFeed();
            foreach (var item in feed.Data.Entries)
            {
                if (item.id == context)
                {
                    _currentEntry = item;
                }
            }
        }

        public void OnViewShown()
        {
            var data = _mapper.Map(_currentEntry);
            _view.DataBind(new WebsiteDetailModel(data));
        }
    }
}