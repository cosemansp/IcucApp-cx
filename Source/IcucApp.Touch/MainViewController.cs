using System.Collections.Generic;
using IcucApp.ViewControllers;
using MonoTouch.UIKit;

namespace IcucApp
{
    public class MainViewController : UITabBarController
    {

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // init tabs
            var tabs = new List<UIViewController>();

            // Add news page tab
            var newsTab = new UINavigationController(new NewsViewController());
            newsTab.TabBarItem = new UITabBarItem("Nieuws", UIImage.FromBundle("house"), 1);
            tabs.Add(newsTab);

            // Add line-up tab
            var regionalNewsTab = new UINavigationController(new LineupViewController());
            regionalNewsTab.TabBarItem = new UITabBarItem("Lineup", UIImage.FromBundle("fire_02"), 2);
            tabs.Add(regionalNewsTab);

            // Add info tab
            var settingsTab = new UINavigationController(new InfoViewController());
            settingsTab.TabBarItem = new UITabBarItem("Info", UIImage.FromBundle("clapboard"), 3);
            tabs.Add(settingsTab);

            // Add info tab
            var ticketTab = new UINavigationController(new TicketViewController());
            ticketTab.TabBarItem = new UITabBarItem("Ticket", UIImage.FromBundle("clapboard"), 4);
            tabs.Add(ticketTab);

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