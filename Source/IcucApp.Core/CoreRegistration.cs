using IcucApp.Core.Ioc;

namespace IcucApp.Core
{
    public class CoreRegistration : IRegistrationModule
    {
        public void Initialize(TinyIoCContainer container)
        {
            // system stuff
            container.Register<ITinyMessengerHub, TinyMessengerHub>().AsSingleton();
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