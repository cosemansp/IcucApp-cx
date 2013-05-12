using System;
using System.Collections.Generic;

namespace IcucApp.Presentation.ViewModels
{
    public class FeedData
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
		public string Excerpt { get; set; }
		public Uri ImageUrl { get; set; }
    }

    public class NewsViewModel
    {
		public static NewsViewModel Empty = new NewsViewModel();
		public static NewsViewModel Loading = new NewsViewModel() { IsLoading = true };

        public NewsViewModel()
        { 
            Entries = new List<FeedData>();
        }

        public List<FeedData> Entries { get; set; }

		public bool IsLoading { get; set; }
    }
}