using IcucApp.Core.Configuration;

namespace IcucApp.Configuration
{
    public class AppSettings : AppSettingsBase
    {
		[BundleKey("App.InfoPageFeed")]
		public string InfoPageFeed { get; set; }

		[BundleKey("App.FacebookFeedUrl")]
		public string FacebookFeedUrl { get; set; }

		[BundleKey("App.NewsFeedUrl")]
		public string NewsFeedUrl { get; set; }

		[BundleKey("App.LineupFeedUrl")]
		public string LineupFeedUrl { get; set; }

		[BundleKey("App.TicketUrl")]
		public string TicketUrl { get; set; }


        public AppSettings()
        {
        }
    }
}