using IcucApp.Core.Configuration;

namespace IcucApp.Configuration
{
    public class AppSettings : AppSettingsBase
    {
        [BundleKey("App.ServerUrl")]
        public string ServerBaseUrl { get; set; }

        public AppSettings()
        {
        }
    }
}