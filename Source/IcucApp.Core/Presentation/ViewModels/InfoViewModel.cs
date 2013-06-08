using System;

namespace IcucApp.Presentation.ViewModels
{
    public class InfoViewModel
    {
		public static InfoViewModel Loading  = new InfoViewModel { IsLoading = true };
		public static InfoViewModel Empty  = new InfoViewModel();

		public bool IsLoading { get; set; }

		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime LastUpdate { get; set; }
		public string FeatureImageUrl { get; set; }

        public string ErrorMessage { get; set; }
    }
}