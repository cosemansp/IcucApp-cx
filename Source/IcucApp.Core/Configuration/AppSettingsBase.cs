using System;

#if DROID
    using Android.App;
    using Android.Content.PM;
    using Concentra.W3.FxMobile.UIAndroid;
    using Android;
#elif TOUCH
using System.Reflection;
using MonoTouch.Foundation;
#endif

namespace IcucApp.Core.Configuration
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BundleKeyAttribute : Attribute
    {
        public string Key { get; private set; }

        public string Default { get; set; }

        public BundleKeyAttribute(string key)
        {
            Key = key;
        }
    }

#if DROID

    public class AppSettingsBase : ICanReadSettings
    {
        private readonly ApplicationInfo _applicationInfo;

        public Version BundleVersion { get; set; }
        public string BundleIdentifier { get; set; }

        public AppSettingsBase()
        {
            _applicationInfo = Application.Context.PackageManager.GetApplicationInfo(Application.Context.PackageName, PackageInfoFlags.MetaData);
        }

        public virtual void Read()
        {
            try
            {
                var properties = GetType().GetProperties(BindingFlags.Instance |
                                                         BindingFlags.Public |
                                                         BindingFlags.FlattenHierarchy);
                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof(BundleKeyAttribute), true);

                    if (attributes.Length > 0)
                    {
                        var bundleAttribute = (BundleKeyAttribute)attributes[0];
                        var textValue = GetString(bundleAttribute.Key);

                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            propertyInfo.SetValue(this, textValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(int))
                        {
                            var intValue = int.Parse(textValue);
                            propertyInfo.SetValue(this, intValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(float))
                        {
                            var intValue = float.Parse(textValue);
                            propertyInfo.SetValue(this, intValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            var dateValue = DateTime.Parse(textValue);
                            propertyInfo.SetValue(this, dateValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(Version))
                        {
                            var versionValue = new Version(textValue);
                            propertyInfo.SetValue(this, versionValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(Uri))
                        {
                            var versionValue = new Uri(textValue);
                            propertyInfo.SetValue(this, versionValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(bool))
                        {
                            var boolValue = GetBoolean(bundleAttribute.Key);
                            propertyInfo.SetValue(this, boolValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(TimeSpan))
                        {
                            var timespanValue = GetTimeSpan(bundleAttribute.Key);
                            propertyInfo.SetValue(this, timespanValue, null);
                        }
                        else
                        {
                            throw new InvalidConfigurationException("Property type not supported: " +
                                                                    propertyInfo.PropertyType);
                        }
                    }
                }
            }
            catch (InvalidConfigurationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidConfigurationException(exception);
            }

            BundleIdentifier = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).PackageName;
            BundleVersion = new Version(Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName);
                //new Version(
                //    Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0)
                //               .VersionCode.ToString(CultureInfo.InvariantCulture));

        }

        public string GetString(string key)
        {
            try
            {
                return _applicationInfo.MetaData.Get(key).ToString();
            }
            catch (Exception)
            {
                throw new InvalidConfigurationException("No metadata found with key: " + key);
            }
        }

        public bool GetBoolean(string key)
        {
            return bool.Parse(GetString(key));
        }

        public int GetInteger(string key)
        {
            return int.Parse(GetString(key));
        }

        public TimeSpan GetTimeSpan(string key)
        {
            return TimeSpan.Parse(GetString(key));
        }
    }

#elif TOUCH 

    public class AppSettingsBase : ICanReadSettings
    {
        [BundleKey("CFBundleVersion")]
        public Version BundleVersion { get; private set; }

        [BundleKey("CFBundleIdentifier")]
        public string BundleIdentifier { get; private set; }

        [BundleKey("MinimumOSVersion")]
        public string MinimumOSVersion { get; private set; }

        public virtual void Read()
        {
            try
            {
                var properties = GetType().GetProperties(BindingFlags.Instance | 
                                                         BindingFlags.Public |
                                                         BindingFlags.FlattenHierarchy);
                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof(BundleKeyAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var bundleAttribute = (BundleKeyAttribute)attributes[0];
						var bundleValue = NSBundle.MainBundle.ObjectForInfoDictionary(bundleAttribute.Key);
						var textValue = bundleValue != null ? bundleValue.ToString() : string.Empty;
						if (propertyInfo.PropertyType == typeof(string))
                        {
                            propertyInfo.SetValue(this, textValue, null);
                        }
						else if (propertyInfo.PropertyType == typeof(int))
                        {
                            var intValue = int.Parse(textValue);
                            propertyInfo.SetValue(this, intValue, null);
                        }
						else if (propertyInfo.PropertyType == typeof(float))
                        {
                            var intValue = float.Parse(textValue);
                            propertyInfo.SetValue(this, intValue, null);
                        }
						else if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            var dateValue = DateTime.Parse(textValue);
                            propertyInfo.SetValue(this, dateValue, null);
                        }
						else if (propertyInfo.PropertyType == typeof(Version))
                        {
                            var versionValue = new Version(textValue);
                            propertyInfo.SetValue(this, versionValue, null);
                        }
						else if (propertyInfo.PropertyType == typeof(Uri))
                        {
                            var versionValue = new Uri(textValue);
                            propertyInfo.SetValue(this, versionValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(bool))
                        {
                            var boolValue = GetBoolean(bundleAttribute.Key);
                            propertyInfo.SetValue(this, boolValue, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(TimeSpan))
                        {
                            var timespanValue = GetTimeSpan(bundleAttribute.Key);
                            propertyInfo.SetValue(this, timespanValue, null);
                        }
                        else
                        {
                            throw new InvalidConfigurationException("Property type not supported: " +
							                                        propertyInfo.PropertyType);
                        }
                    }
                }
            }
            catch (InvalidConfigurationException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidConfigurationException(exception);
            }
        }

        public string GetString(string key)
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary(key).ToString();
        }

        public bool GetBoolean(string key)
        {
            return bool.Parse(GetString(key));
        }

        public int GetInteger(string key)
        {
            return int.Parse(GetString(key));
        }
        
        public TimeSpan GetTimeSpan(string key)
        {
            return TimeSpan.Parse(GetString(key));
        }
    }
#else
    public class AppSettingsBase : ICanReadSettings
    {
        public Version BundleVersion { get; private set; }
        public string BundleIdentifier { get; private set; }
        public string MinimumOSVersion { get; private set; }

        public void Read()
        {
            throw new NotImplementedException();
        }
    }
#endif
}