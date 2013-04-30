using System;
using IcucApp.Configuration;
using IcucApp.Core.Configuration;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Ioc;
using IcucApp.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace IcucApp
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
        private static readonly ILog Log = LogManager.GetLogger("AppDelegate");
	    private UIWindow _mainWindows;
	    private UserSettings _userSettings;
	    private AppSettings _appSettings;

	    // This method is invoked when the application has loaded and is ready to run.
        // You have 17 seconds to return from this method, or iOS will terminate your application.
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
            // Reset badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            // Application setup
            BootStrapper.Configure(app, this);

            // Setup dependencies
		    _userSettings = Container.Resolve<UserSettings>();
            _appSettings = Container.Resolve<AppSettings>();

            // setup main window and its controller
            _mainWindows = new UIWindow(UIScreen.MainScreen.Bounds);  // window based on its screen size
            UIViewController mainController = new MainViewController();
            _mainWindows.RootViewController = mainController;
            _mainWindows.MakeKeyAndVisible();

			return true;
		}

        /// <summary>
        /// When we receive a memory warning, clear the MT.D image cache
        /// </summary>
        public override void ReceiveMemoryWarning(UIApplication application)
        {
            Log.Warn("Received Memory Warning...");

            // clear the MT.D image cache
            //AsyncImageLoader.Purge();

            // for GC
            GC.Collect();
        }

        public override void WillTerminate(UIApplication application)
        {
            Log.Info("WillTerminate...");

            // persist settings
            ConfigurationManager.SaveOrUpdate(_userSettings);

            // Clear image cache & files
            //AsyncImageLoader.Purge(true);
            Log.Info("Terminated");
        }

	    public override void OnActivated(UIApplication application)
	    {
            Log.DebugFormat("OnActivated...");
	    }

	    public override void OnResignActivation(UIApplication application)
	    {
            Log.InfoFormat("OnResignActivation...");

            // persist settings
            ConfigurationManager.SaveOrUpdate(_userSettings);

            // clear the MT.D image cache
            // AsyncImageLoader.Purge();
	    }
	}
}

