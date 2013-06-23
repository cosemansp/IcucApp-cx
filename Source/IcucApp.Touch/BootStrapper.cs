using System.Net;
using IcucApp.Configuration;
using IcucApp.Core;
using IcucApp.Core.Configuration;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Ioc;
using IcucApp.Core.UI;
using IcucApp.Presentation;
using IcucApp.Touch;
using IcucApp.Touch.Core;
using IcucApp.ViewControllers;
using MonoTouch.UIKit;
using IcucApp.Services.PushWoosh;

namespace IcucApp
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
                container.Register<IDispatcher>(new DispatcherTouch(appDelegate));
                container.Register<INavigator>(new TouchNavigator(app));
                container.Register<IPushWooshAgent>(new PushWooshAgent());
            });

			Container.Resolve<IDataLoader>().Start();

            //
            // define routes
            //
            RouteConfig.MapRoute<FacebookDetailPresenter>("FacebookDetail", context => new FacebookDetailViewController(context), true);
			RouteConfig.MapRoute<WebsiteDetailPresenter>("WebsiteDetail", context => new WebsiteDetailViewController(context), true);
            RouteConfig.MapRoute<WebViewPresenter>("WebView", context => new WebViewController(context), true);
        }
    }
}