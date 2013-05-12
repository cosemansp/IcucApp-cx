using IcucApp.Core;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Syndication;
using System;

namespace IcucApp.Presentation.Mappers
{
    public class WordpressEntryMapper : IMapper<WordpressEntry, FeedData>
    {
        public FeedData Map(WordpressEntry source)
        {
           var data = new FeedData
           {
                Type = "wordpress",
                Published = source.date,
 			    Title = System.Web.HttpUtility.HtmlDecode(source.title),
				Excerpt = source.excerpt,
                Content = source.content,
				ImageUrl = (source.image != null && source.image != "False") ? new Uri(source.image) : null
           };
           return data;
        }
    }
}