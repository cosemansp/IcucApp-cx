using System.Collections.Generic;

namespace IcucApp.Core.Services.PushWoosh
{
    public class SetTagsMessage
    {
        public SetTagsMessage(string application, string hwid, Dictionary<string, object> tags)
        {
            request = new SetTagsPayload(application, hwid, tags);
        }

        public SetTagsPayload request { get; set; }
    }
}