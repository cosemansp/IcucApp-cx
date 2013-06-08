namespace IcucApp.Presentation.ViewModels
{
    public class WebsiteDetailModel
    {
        public FeedData Data { get; private set; }

        public WebsiteDetailModel(FeedData data) {
            Data = data;
        }
    }
}