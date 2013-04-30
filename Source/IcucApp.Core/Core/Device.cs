using System;
using System.Runtime.InteropServices;
using System.Text;

#if TOUCH
    using MonoTouch.Foundation;
    using MonoTouch.ObjCRuntime;
    using MonoTouch.UIKit;
#elif DROID
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Telephony;
    using Java.Util;
#else

#endif

namespace IcucApp.Core
{
    public static class Device
    {
#if TOUCH
        [DllImport(MonoTouch.Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);
        private const string HardwareProperty = "hw.machine";
#endif

        public static string Platform
        {
#if TOUCH
            get
            {
                var pLen = Marshal.AllocHGlobal(sizeof(int));
                sysctlbyname(HardwareProperty, IntPtr.Zero, pLen, IntPtr.Zero, 0);
                var length = Marshal.ReadInt32(pLen);
                if (length == 0)
                {
                    Marshal.FreeHGlobal(pLen);
                    return "Unknown";
                }

                var pStr = Marshal.AllocHGlobal(length);
                sysctlbyname(HardwareProperty, pStr, pLen, IntPtr.Zero, 0);

                var hardwareStr = Marshal.PtrToStringAnsi(pStr);
                hardwareStr = hardwareStr ?? "";
                var ret = "Unknown";

                if (hardwareStr == "iPhone1,1")
                    ret = "iPhone";
                else if (hardwareStr == "iPhone1,2")
                    ret = "iPhone 3G";
                else if (hardwareStr == "iPhone2,1")
                    ret = "iPhone 3GS";
                else if (hardwareStr.Contains("iPhone3,"))
                    ret = "iPhone 4";
                else if (hardwareStr == "iPhone4,1")
                    ret = "iPhone 4S";
                else if (hardwareStr.Contains("iPhone5"))
                    ret = "iPhone 5";
                else if (hardwareStr == "iPad1,1")
                    ret = "iPad";
                else if (hardwareStr == "iPad2,1")
                    ret = "iPad 2 (WIFI)";
                else if (hardwareStr == "iPad2,2")
                    ret = "iPad 2 (GSM)";
                else if (hardwareStr == "iPad2,3")
                    ret = "iPad 2 (CDMA)";
                else if (hardwareStr == "iPad2,4")
                    ret = "iPad 2 (WIFI24)";
                else if (hardwareStr == "iPad3,1")
                    ret = "iPad 3 (WIFI)";
                else if (hardwareStr == "iPad3,2")
                    ret = "iPad 3 (GSM)";
                else if (hardwareStr.Contains("iPad"))
                    ret = "iPad";
                else if (hardwareStr == "iPad3,3")
                    ret = "iPad 3CDMA";
                else if (hardwareStr == "iPod1,1")
                    ret = "iPod Touch 1G";
                else if (hardwareStr == "iPod2,1")
                    ret = "iPod Touch 2G";
                else if (hardwareStr == "iPod3,1")
                    ret = "iPod Touch 3G";
                else if (hardwareStr == "iPod4,1")
                    ret = "iPod Touch 4G";
                else if (hardwareStr.Contains("iPod5"))
                    ret = "iPod Touch 5G";
                else if (hardwareStr.Contains("iPod"))
                    ret = "iPod";
                else if (hardwareStr == "i386" || hardwareStr == "x86_64")
                {
                    if (UIDevice.CurrentDevice.Model.Contains("iPhone"))
                        ret = "iPhoneSimulator";
                    else
                        ret = "iPadSimulator";
                }

                Marshal.FreeHGlobal(pLen);
                Marshal.FreeHGlobal(pStr);
                return ret;
            }
#elif DROID
            get
            {
                return Build.Manufacturer + " - " + Build.Product;
            }
#else 
            get { return "unknown"; }
#endif
        }

        public static string Name
        {
#if TOUCH
            get { return UIDevice.CurrentDevice.Name; }
#elif DROID
            get
            {
                // Device model
                return Build.Model;
            }
#else
            get { return System.Environment.MachineName;  }
#endif
        }

        public static string SystemIdentifier
        {
#if TOUCH
            get { return UIDevice.CurrentDevice.UniqueIdentifier; }
#elif DROID
            get
            {
                var tm = (TelephonyManager)Application.Context.GetSystemService(Context.TelephonyService);
                var tmDevice = "" + tm.DeviceId;
                var tmSerial = "" + tm.SimSerialNumber;
                //var androidId = "" + Android.Provider.Settings.Secure.GetString(ContextWrapper.ContentResolver,
                                                                              //Android.Provider.Settings.Secure.AndroidId);
                var androidId = "";
                var deviceUuid = new UUID(androidId.GetHashCode(), ((long)tmDevice.GetHashCode() << 32 | tmSerial.GetHashCode()));
                var deviceId = deviceUuid.ToString();

                return deviceId;
            }
#else
            get { return System.Environment.MachineName.GetHashCode().ToString(); }
#endif
        }

        public static Version SystemVersion
        {
#if TOUCH
            get { return new Version(UIDevice.CurrentDevice.SystemVersion); }
#elif DROID
            get { return new Version(Build.VERSION.Release); }
#else
            get { return System.Environment.Version; }
#endif
        }

#if TOUCH
		public static string DeviceToken
		{
            get
            {
                return NSUserDefaults.StandardUserDefaults.StringForKey("DeviceToken");
            }
            set
            {
                NSUserDefaults.StandardUserDefaults.SetString(value, "DeviceToken");
            }
		}
#elif DROID
        public static string DeviceToken
        {
            get
            {
                var prefs = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
                return prefs.GetString("DeviceToken", String.Empty);
            }
            set
            {
                var prefs = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
                var edit = prefs.Edit();
                edit.PutString("DeviceToken", value);
                edit.Commit();
            }
        }
#else
        private static string _deviceToken;
		public static string DeviceToken
		{
            get { return _deviceToken; }
            set { _deviceToken = value; }
		}
#endif

        public static string UUID
        {
#if TOUCH
            get
            {
                var uuid = NSUserDefaults.StandardUserDefaults.StringForKey("DeviceUUID");
                if (uuid.IsNullOrEmpty())
                {
                    uuid = Guid.NewGuid().ToString();
                    NSUserDefaults.StandardUserDefaults.SetString(uuid, "DeviceUUID");
                }
                return uuid;
            }
#else
            get
            {
                return SystemIdentifier;
            }
#endif
        }

        public static string ShortDescription()
        {
            // return 'iPhone 3GS - v6.0' or 'iPad2Wifi - v5.1.1'
            var sb = new StringBuilder();
            sb.Append(Platform);
            sb.AppendFormat(" - v{0}", SystemVersion);
            return sb.ToString();
        }

        public static bool IsPhone
        {
#if TOUCH
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
#else
            get { return true; }
#endif
        }

        public static bool IsTablet
        {
#if TOUCH
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
            }
#else
            get { return true; }
#endif
        }

        public static bool IsSimulator
        {
#if TOUCH
            get
            {
                return Runtime.Arch == Arch.SIMULATOR;
            }
#elif DROID
            get { return "sdk".Equals(Build.Product); }
#else 
            get { return false; }
#endif
        }
    }
}