using System;

#if DROID
    using Android.Content;
using Android.App;
#elif TOUCH
using System.Globalization;
using System.Reflection;
using MonoTouch.Foundation;
#endif

namespace IcucApp.Core.Configuration
{
#if DROID
    public class UserSettingsbase : ICanReadSettings, ICanSaveSettings
    {
        public virtual void Read()
        {
            try
            {
                var properties = GetType().GetProperties(BindingFlags.Instance |
                                                         BindingFlags.Public |
                                                         BindingFlags.FlattenHierarchy);

                var prefs = Application.Context.GetSharedPreferences("preferences", FileCreationMode.Private);


                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof (BundleKeyAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var bundleAttribute = (BundleKeyAttribute)attributes[0];
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            var value = prefs.GetString(bundleAttribute.Key, null);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(int))
                        {
                            var value = prefs.GetInt(bundleAttribute.Key, 0);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(float))
                        {
                            var value = prefs.GetFloat(bundleAttribute.Key, 0);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(bool))
                        {
                            var value = prefs.GetBoolean(bundleAttribute.Key, false);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                        {

                            var value = prefs.GetString(bundleAttribute.Key, null);
                            if (value != null)
                            {
                                var dateValue = DateTime.ParseExact(value, "yy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture);
                                propertyInfo.SetValue(this, dateValue, null);
                            }
                            else
                                propertyInfo.SetValue(this, DateTime.MinValue, null);
                        }
                        else
                        {
                            throw new InvalidConfigurationException("Property type not supported: " +
                                                                    propertyInfo.DeclaringType);
                        }
                    }
                }
            }
            catch (InvalidConfigurationException)
            {
                throw;
            }
            catch(Exception exception)
            {
                throw new InvalidConfigurationException(exception);
            }
        }

        public virtual void SaveOrUpdate()
        {
            try
            {
                var properties = GetType().GetProperties(BindingFlags.Instance |
                                                         BindingFlags.Public |
                                                         BindingFlags.FlattenHierarchy);

                var prefs = Application.Context.GetSharedPreferences("preferences", FileCreationMode.Append);
                var editor = prefs.Edit();

                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof (BundleKeyAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var bundleAttribute = (BundleKeyAttribute) attributes[0];
                        if (propertyInfo.PropertyType == typeof (string))
                        {
                            var value = propertyInfo.GetValue(this, null) as string ?? "";
                            editor.PutString(bundleAttribute.Key, value);
                        }
                        else if (propertyInfo.PropertyType == typeof (int))
                        {
                            editor.PutInt(bundleAttribute.Key, (int)propertyInfo.GetValue(this, null));
                        }
                        else if (propertyInfo.PropertyType == typeof (float))
                        {
                            editor.PutFloat(bundleAttribute.Key, (float)propertyInfo.GetValue(this, null));
                        }
                        else if (propertyInfo.PropertyType == typeof (bool))
                        {
                            editor.PutBoolean(bundleAttribute.Key, (bool)propertyInfo.GetValue(this, null));
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            var dateValue = (DateTime) propertyInfo.GetValue(this, null);
                            editor.PutString(bundleAttribute.Key, dateValue.ToString("yy-MM-ddThh:mm:ss"));
                        }
                        else
                        {
                            throw new InvalidConfigurationException("Property type not supported: " +
                                                                    propertyInfo.PropertyType);
                        }
                    }
                }

                editor.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR writing config: " + ex.Message);
            }
        }

    }
#elif TOUCH 
    public class UserSettingsbase : ICanReadSettings, ICanSaveSettings
    {
        public virtual void Read()
        {
            try
            {
                var properties = GetType().GetProperties(BindingFlags.Instance |
                                                         BindingFlags.Public |
                                                         BindingFlags.FlattenHierarchy);
                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof (BundleKeyAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var bundleAttribute = (BundleKeyAttribute)attributes[0];
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            var value = NSUserDefaults.StandardUserDefaults.StringForKey(bundleAttribute.Key);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(int))
                        {
                            var value = NSUserDefaults.StandardUserDefaults.IntForKey(bundleAttribute.Key);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(float))
                        {
                            var value = NSUserDefaults.StandardUserDefaults.FloatForKey(bundleAttribute.Key);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(bool))
                        {
                            var value = NSUserDefaults.StandardUserDefaults.BoolForKey(bundleAttribute.Key);
                            propertyInfo.SetValue(this, value, null);
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            var value = NSUserDefaults.StandardUserDefaults.StringForKey(bundleAttribute.Key);
                            if (value != null)
                            {
                                var dateValue = DateTime.ParseExact(value, "yy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture);
                                propertyInfo.SetValue(this, dateValue, null);
                            }
                            else
                                propertyInfo.SetValue(this, DateTime.MinValue, null);
                        }
                        else
                        {
                            throw new InvalidConfigurationException("Property type not supported: " +
                                                                    propertyInfo.DeclaringType);
                        }
                    }
                }
            }
            catch (InvalidConfigurationException)
            {
                throw;
            }
            catch(Exception exception)
            {
                throw new InvalidConfigurationException(exception);
            }
        }

        public virtual void SaveOrUpdate()
        {
            try
            {
                var properties = GetType().GetProperties(BindingFlags.Instance |
                                                         BindingFlags.Public |
                                                         BindingFlags.FlattenHierarchy);
                foreach (var propertyInfo in properties)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof (BundleKeyAttribute), true);
                    if (attributes.Length > 0)
                    {
                        var bundleAttribute = (BundleKeyAttribute) attributes[0];
                        if (propertyInfo.PropertyType == typeof (string))
                        {
                            var value = propertyInfo.GetValue(this, null) as string ?? "";
                            NSUserDefaults.StandardUserDefaults.SetString(value,
                                                                          bundleAttribute.Key);
                        }
                        else if (propertyInfo.PropertyType == typeof (int))
                        {
                            NSUserDefaults.StandardUserDefaults.SetInt((int) propertyInfo.GetValue(this, null),
                                                                       bundleAttribute.Key);
                        }
                        else if (propertyInfo.PropertyType == typeof (float))
                        {
                            NSUserDefaults.StandardUserDefaults.SetFloat((float) propertyInfo.GetValue(this, null),
                                                                         bundleAttribute.Key);
                        }
                        else if (propertyInfo.PropertyType == typeof (bool))
                        {
                            NSUserDefaults.StandardUserDefaults.SetBool((bool) propertyInfo.GetValue(this, null),
                                                                        bundleAttribute.Key);
                        }
                        else if (propertyInfo.PropertyType == typeof(DateTime))
                        {
                            var dateValue = (DateTime) propertyInfo.GetValue(this, null);
                            NSUserDefaults.StandardUserDefaults.SetString(dateValue.ToString("yy-MM-ddThh:mm:ss"),
                                                                          bundleAttribute.Key);
                        }
                        else
                        {
                            throw new InvalidConfigurationException("Property type not supported: " +
                                                                    propertyInfo.PropertyType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR writing config: " + ex.Message);
            }
        }

    }
#else
    public class UserSettingsbase : ICanReadSettings, ICanSaveSettings
    {
        public void Read()
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdate()
        {
            throw new NotImplementedException();
        }
    }
#endif
}