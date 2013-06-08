using IcucApp.Core.UI;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Syndication;
using IcucApp.Services.Facebook;
using IcucApp.Core;

namespace IcucApp.Presentation
{
    public interface IFacebookDetailView : IView
    {
        void DataBind(FacebookDetailModel model);
    }

    public class FacebookDetailPresenter : IPresenter
    {
        private IFacebookDetailView _view;
        private FacebookEntry _currentEntry;
        private IMapper<FacebookEntry, FeedData> _mapper;
        private ICache _cache;

        public FacebookDetailPresenter(IFacebookDetailView view, 
                                       ICache cache,
                                       IMapper<FacebookEntry, FeedData> facebookMapper) 
        {
            _view = view;
            _cache = cache;
            _mapper = facebookMapper;
        }

        public void Initialize(string context)
        {
            var feed = _cache.GetFacebookFeed();
            foreach (var item in feed.Data.entries)
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
            _view.DataBind(new FacebookDetailModel(data));
        }
    }

   
}