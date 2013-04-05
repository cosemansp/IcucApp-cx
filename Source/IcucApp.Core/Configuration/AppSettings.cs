using System;

namespace IcucApp.Core.Configuration
{
    public class AppSettings : AppSettingsBase
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        [BundleKey("App.ServerUrl")]
        public string ServerBaseUrl { get; private set; }

        public AppSettings()
        {
        }
    }

    // ReSharper restore UnusedAutoPropertyAccessor.Local
}