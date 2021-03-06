using System.Collections.Generic;
using RestSharp;

namespace IcucApp.Services.PushWoosh
{
    public class SetTagsPayload
    {
        public SetTagsPayload(string application, string hwid, Dictionary<string, object> tags)
        {
            this.application = application;
            this.hwid = hwid;
            this.tags = new JsonObject();
            foreach (var key in tags.Keys)
            {
                object value = tags[key];
                this.tags[key] = value;
            }
        }

        // ReSharper disable InconsistentNaming
        public string application { get; private set; }
        public string hwid { get; private set; }
        public JsonObject tags { get; private set; }
        // ReSharper restore InconsistentNaming
    }
}