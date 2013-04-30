using System.Collections.Generic;

namespace IcucApp.Services.PushWoosh
{
    public class SetTagsMessage
    {
        public SetTagsMessage(string application, string hwid, Dictionary<string, object> tags)
        {
            request = new SetTagsPayload(application, hwid, tags);
        }

        // ReSharper disable InconsistentNaming
        public SetTagsPayload request { get; set; }
        // ReSharper restore InconsistentNaming
    }
}