using IcucApp.Core;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Syndication;

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
               Content = source.excerpt
           };
           return data;
        }
    }
}