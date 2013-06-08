using System.Collections.Generic;

namespace IcucApp.Presentation.ViewModels
{
    public class LineupViewModel
    {
		public static LineupViewModel Empty = new LineupViewModel();
		public static LineupViewModel Loading = new LineupViewModel() { IsLoading = true };
		
		public LineupViewModel()
		{ 
			Entries = new List<FeedData>();
		}
		
		public List<FeedData> Entries { get; set; }
		
		public bool IsLoading { get; set; }

        public string ErrorMessage { get; set; }
    }
}