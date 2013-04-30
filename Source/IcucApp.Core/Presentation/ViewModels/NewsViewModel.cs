using System;
using System.Collections.Generic;

namespace IcucApp.Presentation.ViewModels
{
    public class FeedData
    {
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }

    public class NewsViewModel
    {
        public NewsViewModel()
        {
            Entries = new List<FeedData>();
        }

        public List<FeedData> Entries { get; set; }
    }
}