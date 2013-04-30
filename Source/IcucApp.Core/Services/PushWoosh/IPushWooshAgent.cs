using System.Collections.Generic;

namespace IcucApp.Services.PushWoosh
{
    public interface IPushWooshAgent
    {
        void RegisterDevice(string appCode, string deviceToken, string hardwareId);
        void UnregisterDevice(string appCode, string hardwareId);
        void SetTags(string appCode, string hardwareId, Dictionary<string, object> tags);
        void GetNearestZone(string appCode, string hardwareId, string lat, string lng);
    }
}