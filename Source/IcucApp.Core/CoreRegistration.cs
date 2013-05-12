using IcucApp.Core.Ioc;
using IcucApp.Presentation.Mappers;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Facebook;
using IcucApp.Services.Syndication;

namespace IcucApp.Core
{
    public class CoreRegistration : IRegistrationModule
    {
        public void Initialize(TinyIoCContainer container)
        {
            // system stuff
            container.Register<ITinyMessengerHub, TinyMessengerHub>().AsSingleton();

            // agents 
            container.Register<IFacebookFeedAgent, FacebookFeedAgent>();
			container.Register<IWordpressFeedAgent>(new WordpressFeedAgent("http://icuc.be"));

            // mappers
            container.Register<IMapper<FacebookEntry, FeedData>, FacebookEntryMapper>();
			container.Register<IMapper<WordpressEntry, FeedData>, WordpressEntryMapper>();

            //container.Register<IAnalyticsTracker, AnalyticsTracker>().AsSingleton();
            //container.Register<IWebBrowser, WebBrowser>().AsSingleton();
#if DROID
    //container.Register<IFileSystem, DroidFileSystem>();
    //container.Register<IImageLoader, DroidImageLoader>();
#elif TOUCH
            //container.Register<IFileSystem, TouchFileSystem>();
            //container.Register<IImageLoader, TouchImageLoader>();
#endif
        }
    }
}