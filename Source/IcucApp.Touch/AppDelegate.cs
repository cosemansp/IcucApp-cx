using System;
using IcucApp.Configuration;
using IcucApp.Core.Configuration;
using IcucApp.Core.Diagnostics;
using IcucApp.Core.Ioc;
using IcucApp.Touch;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using IcucApp.Core;
using MonoTouch.AudioToolbox;
using IcucApp.Core.Touch.UIKit;
using MonoTouch.Dialog.Utilities;
using MonoTouch.ObjCRuntime;
using IcucApp.Services.PushWoosh;

namespace IcucApp
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
        private static readonly ILog Log = LogManager.GetLogger("AppDelegate");
	    private UIWindow _mainWindows;
	    private UserSettings _userSettings;
        private IPushWooshAgent _pushwooshAgent;
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
            _pushwooshAgent = Container.Resolve<IPushWooshAgent>();
            _appSettings = Container.Resolve<AppSettings>();

            // setup main window and its controller
            _mainWindows = new UIWindow(UIScreen.MainScreen.Bounds);  // window based on its screen size
            UIViewController mainController = new MainViewController();
            _mainWindows.RootViewController = mainController;
            _mainWindows.MakeKeyAndVisible();

            // setup remote notification
            if (Device.IsSimulator) {
                // RegisterDeviceOnApplicationServer("0f744707bebcf74f9b7c25d48e3358945f6aa01da5ddb387462c7eaf61bbad78" /* sample token */);
                return true; // don't register when is running in simulator
            }

            // Go ahead and ask the user for permission to use Push Notifications
            UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert | UIRemoteNotificationType.Sound);

            // if the options is null we can stop here.
            if (options == null)
                return true;

            // If the user clicked the 'view' button on the notification
            // to launch the application we have to handle them here
            ProcessNotification(options, true);

            // finish successfull
			return true;
		}

        /// <summary>
        /// When we receive a memory warning, clear the MT.D image cache
        /// </summary>
        public override void ReceiveMemoryWarning(UIApplication application)
        {
            Log.Warn("Received Memory Warning...");

            // clear the MT.D image cache
            ImageLoader.Purge();

            // for GC
            GC.Collect();
        }

        public override void WillTerminate(UIApplication application)
        {
            Log.Info("WillTerminate...");

            // persist settings
            ConfigurationManager.SaveOrUpdate(_userSettings);
        }

	    public override void OnActivated(UIApplication application)
	    {
            Log.DebugFormat("OnActivated...");

            // Reset badge
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
	    }

	    public override void OnResignActivation(UIApplication application)
	    {
            Log.InfoFormat("OnResignActivation...");

            // persist settings
            ConfigurationManager.SaveOrUpdate(_userSettings);
	    }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Log.InfoFormat("RegisteredForRemoteNotifications...");
            try
            {
                // The deviceToken is of interest here, this is what your push notification server needs to send out a notification
                // to the device.  So, most times you'd want to send the device Token to your servers when it has changed
                var str = (NSString) Runtime.GetNSObject(Messaging.intptr_objc_msgSend(deviceToken.Handle,
                                                                                       new Selector("description").Handle));
                var newDeviceToken = str.ToString().Replace("<", "").Replace(">", "").Replace(" ", "");

                // Get last token from settings
                Log.InfoFormat("RegisteredForRemoteNotifications - received token: '{0}'", newDeviceToken);
                Log.InfoFormat("                                   current token: '{0}'", Device.DeviceToken);
                Log.InfoFormat("                                   UUID: '{0}'", Device.UUID);

                // Register deviceToken to applicationServer
                if (String.IsNullOrEmpty(Device.DeviceToken) || Device.DeviceToken != newDeviceToken)
                {
                    // only register when deviceToken is changed
                    Log.InfoFormat("DeviceToken empty of changed, register device to app server");
                    RegisterDeviceOnApplicationServer(newDeviceToken);
                }
            }
            catch (Exception exception)
            {
                Log.ErrorFormat("Failed to register device: {0}", exception.Message);
            }
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            // This method gets called whenever the app is already running and receives a push notification
            // YOU MUST HANDLE the notifications in this case.  Apple assumes if the app is running, it takes care of everything
            // this includes setting the badge, playing a sound, etc.

            Log.Info("ReceivedRemoteNotification...");
            ProcessNotification(userInfo, false);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            // Registering for remote notifications failed for some reason
            // This is usually due to your provisioning profiles not being properly setup in your project options
            // or not having the right mobileprovision included on your device
            // or you may not have setup your app's product id to match the mobileprovision you made
            Log.WarnFormat("  Failed to Register for Remote Notifications - Domain: {0}, Code:{1}, Desc: {2}",
                           error.Domain, error.Code, error.LocalizedDescription);

            #if DEBUG
                UIAlertHelper.ShowAlert("ErrorFormat registering push notifications", error.LocalizedDescription);
            #endif
        }

        private void RegisterDeviceOnApplicationServer(string deviceToken)
        {
            Log.InfoFormat("OnRegisterDeviceOnApplicationServer: token: {0}", deviceToken);
            try {
                // register device to pushwoosh
                _pushwooshAgent.RegisterDevice(_appSettings.PushWooshAppCode, deviceToken, Device.UUID);

                // when successfull, save devieToken
                NSUserDefaults.StandardUserDefaults.SetString(deviceToken, "DeviceToken");
            }
            catch(Exception ex) {
                Log.ErrorFormat("  Failed to register device to pushWoosh", ex.Message);
            }
        }

        private void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // The options variable will contain our notification data if the user clicked the 'view' button on the notification
            // to launch the application.  So process it here. 
            if (options == null)
                return;

            // check to see if the dictionary has the aps key.  This is the notification payload you would have sent
            if (options.ContainsKey(new NSString("aps")))
            {
                //Get the aps dictionary
                var aps = (NSDictionary) options.ObjectForKey(new NSString("aps"));
                string alert = string.Empty;
                string sound = string.Empty;
                string custom = string.Empty;
                int badge = -1;

                //Extract the alert text
                //NOTE: If you're using the simple alert by just specifying "  aps:{alert:"alert msg here"}  "
                //      this will work fine.  But if you're using a complex alert with Localization keys, etc., your "alert" object from the aps dictionary
                //      will be another NSDictionary... Basically the json gets dumped right into a NSDictionary, so keep that in mind
                if (aps.ContainsKey(new NSString("alert")))
                    alert = (aps[new NSString("alert")]).ToString();

                //Extract the sound string
                if (aps.ContainsKey(new NSString("sound")))
                    sound = (aps[new NSString("sound")]).ToString();

                //Extract the badge
                if (aps.ContainsKey(new NSString("badge")))
                {
                    string badgeStr = (aps[new NSString("badge")]).ToString();
                    int.TryParse(badgeStr, out badge);
                }

                if (aps.ContainsKey(new NSString("custom")))
                    custom = (aps[new NSString("custom")]).ToString();

                // If this came from the ReceivedRemoteNotification while the app was running,
                // we of course need to manually process things like the sound, badge, and alert.
                if (!fromFinishedLaunching)
                {
                    // Manually set the badge in case this came from a remote notification sent while the app was open
                    if (badge >= 0)
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = badge;

                    // Manually play the sound
                    if (!string.IsNullOrEmpty(sound))
                    {
                        // This assumes that in your json payload you sent the sound filename (like sound.caf)
                        // and that you've included it in your project directory as a Content Build type.
                        SystemSound soundObj = SystemSound.FromFile(sound);
                        if (soundObj != null)
                            soundObj.PlaySystemSound();
                    }

                    // Manually show an alert
                    Log.InfoFormat("ReceivedRemoteNotification: alert: {0}, custom: {1}", alert, custom);

                    // don't display anything
                    // UIAlertHelper.ShowAlert("Notification", alert, "Ok");
                }
            }
        }
	}
}

