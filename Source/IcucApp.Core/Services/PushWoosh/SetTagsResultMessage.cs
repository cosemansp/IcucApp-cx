using System.Collections.Generic;

namespace IcucApp.Core.Services.PushWoosh
{
    public class SetTagsResultMessage : StatusMessage
    {
        public SetTagsResponse response { get; set; }
    }

    public class SetTagsResponse
    {
        public List<TagError> skipped { get; set; }
    }

    public class TagError
    {
        public string tag { get; set; }
        public string reason { get; set; }
    }
}