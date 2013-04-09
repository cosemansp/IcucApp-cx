using System;
using System.Collections.Generic;

namespace IcucApp.Core.Presentation.ViewModels
{
    public class FeedData
    {
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }

    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Entries = new List<FeedData>();
        }

        public List<FeedData> Entries { get; set; }
    }
}