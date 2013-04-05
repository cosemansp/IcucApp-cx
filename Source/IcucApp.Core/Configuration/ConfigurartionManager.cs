
#if DROID

#elif TOUCH
using System.IO;
using MonoTouch.Foundation;
#endif

namespace IcucApp.Core.Configuration
{
    public interface ICanSaveSettings
    {
        void SaveOrUpdate();
    }
    
    public interface ICanReadSettings
    {
        void Read();
    }

    public interface IWantRegisterDefaults
    {

    }

    public class ConfigurationManager
    {
        public static void RegisterDefaultsFromSettingsBundle()
        {
#if TOUCH
            string settingsBundle = NSBundle.MainBundle.PathForResource("Settings", @"bundle");
            if (settingsBundle == null)
                return;

            var keyString = new NSString(@"Key");
            var defaultString = new NSString(@"DefaultValue");
            var settings = NSDictionary.FromFile(Path.Combine(settingsBundle, @"Root.plist"));
            var preferences = (NSArray)settings.ValueForKey(new NSString(@"PreferenceSpecifiers"));
            var defaultsToRegister = new NSMutableDictionary();
            for (uint i = 0; i < preferences.Count; i++)
            {
                var prefSpecification = new NSDictionary(preferences.ValueAt(i));
                var key = (NSString)prefSpecification.ValueForKey(keyString);
                if (key != null)
                {
                    var def = prefSpecification.ValueForKey(defaultString);
                    if (def != null)
                    {
                        defaultsToRegister.SetValueForKey(def, key);
                    }
                }
            }
            NSUserDefaults.StandardUserDefaults.RegisterDefaults(defaultsToRegister);
#endif
        }

        /// <summary>
        /// <![CDATA[
        /// var appSettings = ConfigurationManager.GetSettings<AppSettings>();
        /// ]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetSettings<T>() where T : class, new()
        {
            var settingsObj = new T();
            var readableSettings = settingsObj as ICanReadSettings;
            if (readableSettings != null)
                readableSettings.Read();

            var registerDefaults = settingsObj as IWantRegisterDefaults;
            if (registerDefaults != null)
                RegisterDefaultsFromSettingsBundle();
            return settingsObj;
        }

        /// <summary>
        /// <![CDATA[
        /// ConfigurationManager.SaveOrUpdate(userSettings);
        /// ]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void SaveOrUpdate<T>(T settings) where T : class
        {
            var updatableSettings = settings as ICanSaveSettings;
            if (updatableSettings != null)
                updatableSettings.SaveOrUpdate();
        }
    }
}