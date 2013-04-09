using IcucApp.Core.Presentation.ViewModels;
using IcucApp.Core.Services.Facebook;

namespace IcucApp.Core.Mappers
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