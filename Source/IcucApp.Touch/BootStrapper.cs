using System.Net;
using IcucApp.Core;
using IcucApp.Core.Configuration;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Ioc;
using IcucApp.Core.Presentation;
using IcucApp.Core.Services;
using IcucApp.Touch.ViewControllers;
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
                container.Register<INavigator>(new TouchNavigator(app));
                //container.Register<IFileSystem, TouchFileSystem>().AsSingleton();
                //container.Register<IAssets, TouchAssets>().AsSingleton();
                //container.Register<ISocialSharing, CustomSocicalSharing>().AsSingleton();
            });

            //
            // define routes
            //
            RouteConfig.MapRoute<FacebookDetailPresenter>("FacebookDetail", context => new FacebookDetailViewController(context), true);
        }
    }
}