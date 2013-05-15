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
            newsTab.TabBarItem = new UITabBarItem("Nieuws", UIImage.FromBundle("home"), 1);
            tabs.Add(newsTab);

            // Add line-up tab
            var lineupTab = new UINavigationController(new LineupViewController());
            lineupTab.TabBarItem = new UITabBarItem("Lineup", UIImage.FromBundle("star"), 2);
            tabs.Add(lineupTab);

            // Add info tab
            var infoTab = new UINavigationController(new InfoViewController());
            infoTab.TabBarItem = new UITabBarItem("Info", UIImage.FromBundle("info"), 3);
            tabs.Add(infoTab);

            // Add info tab
            var ticketTab = new UINavigationController(new TicketViewController());
            ticketTab.TabBarItem = new UITabBarItem("Ticket", UIImage.FromBundle("ticket"), 4);
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