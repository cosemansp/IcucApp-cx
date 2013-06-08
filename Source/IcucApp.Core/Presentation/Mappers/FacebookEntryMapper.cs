using IcucApp.Core;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Facebook;

namespace IcucApp.Presentation.Mappers
{
    public class FacebookEntryMapper : IMapper<FacebookEntry, FeedData>
    {
        public FeedData Map(FacebookEntry source)
        {
            var data = new FeedData
            {
                Type = "facebook",
                Author = source.author.name,
				Content = System.Web.HttpUtility.HtmlDecode(source.content),
                Published = source.published,
				Title = System.Web.HttpUtility.HtmlDecode(source.title),
                Id = source.id,
                Updated = source.updated
            };
            return data;
        }
    }
}