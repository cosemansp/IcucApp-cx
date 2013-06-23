using IcucApp.Core.Configuration;
using System;

namespace IcucApp.Configuration
{
    public class AppSettings : AppSettingsBase
    {
        [BundleKey("App.TimeoutContent")]
        public TimeSpan TimeoutContent { get; set; }

        [BundleKey("App.PushWooshAppCode")]
        public string PushWooshAppCode { get; set;}

        public AppSettings()
        {
        }
    }
}