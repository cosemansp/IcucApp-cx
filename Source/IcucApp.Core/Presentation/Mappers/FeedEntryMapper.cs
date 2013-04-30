using IcucApp.Core;
using IcucApp.Presentation.ViewModels;
using IcucApp.Services.Facebook;

namespace IcucApp.Presentation.Mappers
{
    public class FeedEntryMapper : IMapper<FeedEntry, FeedData>
    {
        public FeedData Map(FeedEntry source)
        {
            var data = new FeedData
            {
                Author = source.author.name,
                Content = source.content,
                Published = source.published,
                Title = source.title,
                Updated = source.updated
            };
            return data;
        }
    }
}