using System;

namespace IcucApp.Presentation.ViewModels
{
    public class FacebookDetailModel
    {
        public FeedData Data { get; private set; }

        public FacebookDetailModel(FeedData data) {
            Data = data;
        }
    }
}