#if TOUCH
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
#endif

#if DROID
using Android.App;
using Android.Content.Res;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Content;
#endif

namespace IcucApp.Core
{
    public static class Display
    {
#if TOUCH
        // Sizes
        // http://www.idev101.com/code/User_Interface/sizes.html
        //
        public const float TabBarHeight = 49;
        public const float NavigationBarHeight = 44;
        public const float StatusBarHeight = 20;
        public const float WindowHeight = 460;
        public const float WindowHeightTall = 568;
        public const float ScreenHeight = 480;
        public const float ScreenHeightTall = 568;
        public const float ScreenWidth = 320;

        public static bool IsTall
        {
            get
            {
                return (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone &&
                       (UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale >= 1136));
            }
        }

        public static bool IsHighRes
        {
            get
            {
                if (UIScreen.MainScreen.RespondsToSelector(new Selector("scale")))
                    return (UIScreen.MainScreen.Scale > 1.0);
                return false;
            }
        }


        public static double ImageDpiMultiplier
        {
            get
            {
                return IsHighRes ? 2 : 1;
            }
        }

        public static double Width
        {
            get { return ScreenWidth; }
        }

        public static double Height
        {
            get
            {
                return IsTall ? WindowHeightTall : WindowHeight;
            }
        }
#elif DROID
        public static double ImageDpiMultiplier
        {
            get
            {
                switch (Resources.System.DisplayMetrics.DensityDpi)
                {
                    case DisplayMetricsDensity.Low:
                        return 0.75;
                    case DisplayMetricsDensity.Medium:
                        return 1;
                    case DisplayMetricsDensity.High:
                        return 1.5;
                    case DisplayMetricsDensity.Xhigh:
                        return 1.33;
                }
                return 1;
            }
        }

        public static double Width
        {
            get
            {
                var mWindowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                return mWindowManager.DefaultDisplay.Width;
            }
        }

        public static double Height
        {
            get
            {
                var mWindowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                return mWindowManager.DefaultDisplay.Height;
            }
        }
#else
        public static double ImageDpiMultiplier
        {
            get
            {
                return 1;
            }
        }

        public static double Width
        {
            get
            {
                return 1;
            }
        }

        public static double Height
        {
            get
            {
                return 1;
            }
        }
#endif
    }
}