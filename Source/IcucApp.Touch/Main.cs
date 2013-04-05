using System;
using System.Collections.Generic;
using IcucApp.Core;
using IcucApp.Core.Diagnostics;
using MonoTouch.UIKit;

namespace IcucApp.Touch
{
	public class Application
	{
        private static readonly ILog Log = LogManager.GetLogger(typeof(Application));

		// This is the main entry point of the application.
		static void Main (string[] args)
		{
#if (DEBUG || TESTING) 
            // Setup logging
            LogManager.AddAppender(new ConsoleAppender());
            LogManager.RootLevel = LogLevel.Info;
		    //LogManager.Filter = ".*Presenter";
#endif

            // unhandled exceptions 
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

		    DumpFonts();

            // Activate AppDelegate
            try
            {
                Log.InfoFormat("Application startup: {0}", Device.ShortDescription());
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception exception)
            {
                Log.Fatal("General failure in app", exception);
                throw;
            }
		}

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal("Unhandled exception", e.ExceptionObject as Exception);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        static void DumpFonts()
        {
            String fonts = "";
            var fontFamilies = new List<String>(UIFont.FamilyNames);
            fontFamilies.Sort();
            foreach (String familyName in fontFamilies)
            {
                foreach (String fontName in UIFont.FontNamesForFamilyName(familyName))
                {
                    fonts += fontName + "\n";
                }
                fonts += "\n";
            }
            Console.WriteLine(fonts);
        }
	}
}
