using System.Net;
using IcucApp.Core;
using IcucApp.Core.Configuration;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Ioc;
using MonoTouch.UIKit;

namespace IcucApp.Touch
{
    public class BootStrapper
    {
        private static AppSettings _appSettings;
        private static UserSettings _userSettings;
        private static readonly ILog Log = LogManager.GetLogger(typeof (BootStrapper).Name);

        public static void Configure(UIApplication app, UIApplicationDelegate appDelegate)
        {
            // To not depend on the root certficates, we will
            // accept any certificates:
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, ssl) => true;

            // setup UX appearance
            Appearance.Configure();

            //
            // Get configuration
            //
            _appSettings = ConfigurationManager.GetSettings<AppSettings>();
            _userSettings = ConfigurationManager.GetSettings<UserSettings>();
            //Log.InfoFormat("BackgroundRetryTime: {0}", _appSettings.BackgroundRetryTime);


            //
            // setup IOC container
            //
            Container.Initialize(container =>
            {
                // settings
                container.Register(_appSettings);
                container.Register(_userSettings);

                // system stuff
                container.RegisterModule(new CoreRegistration());

                // local services
                container.Register<IDispatcher>(new Dispatcher(appDelegate));
                //container.Register<INavigator>(new TouchNavigator(app));
                //container.Register<IFileSystem, TouchFileSystem>().AsSingleton();
                //container.Register<IAssets, TouchAssets>().AsSingleton();
                //container.Register<ISocialSharing, CustomSocicalSharing>().AsSingleton();
            });

            //
            // define routes
            //
            //RouteConfig.MapRoute<ArticlePresenter>("ArticlePage", context => new ArticleWebViewController(context), true);
            //RouteConfig.MapRoute<LoginPresenter>("LoginPage", context => new LoginViewController(context), true);
            //RouteConfig.MapRoute<SectionPresenter>("SectionPage", context => new SectionViewController(context), true);
            //RouteConfig.MapRoute<CommentsPresenter>("CommentsPage", context => new CommentsViewController(context), true);
            //RouteConfig.MapRoute<SelectRegionPresenter>("SelectRegioPage", context => new SelectRegionViewController(context), true);
        }
    }
}