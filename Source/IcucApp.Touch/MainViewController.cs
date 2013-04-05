using System.Collections.Generic;
using IcucApp.Touch.ViewControllers;
using MonoTouch.UIKit;

namespace IcucApp.Touch
{
    public class MainViewController : UITabBarController
    {

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // init tabs
            var tabs = new List<UIViewController>();

            // Add home page tab
            var newsTab = new UINavigationController(new HomeViewController());
            newsTab.TabBarItem = new UITabBarItem("Home", UIImage.FromBundle("house"), 1);
            tabs.Add(newsTab);

            // Add regional news tab
            var regionalNewsTab = new UINavigationController(new Tab2ViewController());
            regionalNewsTab.TabBarItem = new UITabBarItem("Tab2", UIImage.FromBundle("fire_02"), 2);
            tabs.Add(regionalNewsTab);


            // Add settings tab
            var settingsTab = new UINavigationController(new Tab3ViewController());
            settingsTab.TabBarItem = new UITabBarItem("Tab3", UIImage.FromBundle("clapboard"), 3);
            tabs.Add(settingsTab);

            // Assign to view controllers.
            ViewControllers = tabs.ToArray();
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        public override bool ShouldAutorotate()
        {
            return false;
        }
    }
}