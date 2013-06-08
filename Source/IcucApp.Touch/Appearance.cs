using MonoTouch.UIKit;

namespace IcucApp.Touch
{
    public static class Appearance
    {
         public static void Configure()
         {

			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.BlackTranslucent, true);

            UINavigationBar.Appearance.TintColor = UIColor.DarkGray;

             // setup tint and font
			 //UITabBar.Appearance.TintColor = UIColor.Green;
             //UINavigationBar.AppearanceWhenContainedIn(typeof(UINavigationController)).TintColor = UIColor.Green;
             //UITabBar.Appearance.TintColor = AppStyle.Color.NavBarTint;
             //UIActionSheet.Appearance.BackgroundColor = AppStyle.Color.NavBarTint;

             //// set title font and color
             //var titleAttr = new UITextAttributes
             //{
             //    TextShadowColor = UIColor.Clear,
             //    TextColor = UIColor.Black,
             //    TextShadowOffset = new UIOffset(0.0f, 0.0f)
             //};
             //UINavigationBar.AppearanceWhenContainedIn(typeof(UINavigationController)).SetTitleTextAttributes(titleAttr);

             //// set button font and color
             //var buttonTextAttr = new UITextAttributes
             //{
             //    TextShadowColor = UIColor.Clear,
             //    TextColor = UIColor.Black,
             //    TextShadowOffset = new UIOffset(0.0f, 0.0f)
             //};
             //UIBarButtonItem.AppearanceWhenContainedIn(typeof(UINavigationController)).TintColor = AppStyle.Color.NavBarButtonTint;
             //UIBarButtonItem.AppearanceWhenContainedIn(typeof(UINavigationController)).SetTitleTextAttributes(buttonTextAttr, UIControlState.Normal);
         }
    }
}