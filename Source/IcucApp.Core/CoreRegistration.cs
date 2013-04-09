using IcucApp.Core.Ioc;
using IcucApp.Core.Mappers;
using IcucApp.Core.Presentation.ViewModels;
using IcucApp.Core.Services.Facebook;

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

            // mappers
            container.Register<IMapper<FeedEntry, FeedData>, FeedEntryMapper>();

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