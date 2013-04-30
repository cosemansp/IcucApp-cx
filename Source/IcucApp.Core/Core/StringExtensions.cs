using System;

namespace IcucApp.Core
{
    public static class StringExtensions
    {
         public static bool IsNullOrEmpty(this string source)
         {
             return string.IsNullOrEmpty(source);
         }

        public static string FormatWith(this string format, params object[] args)
        {
            try
            {
                if (format.IsNullOrEmpty())
                    return string.Empty;
                return string.Format(format, args);
            }
            catch(Exception exception)
            {
                return "Format Exception: " + format + ", error: " + exception.Message;
            }
        }

        public static bool IsValidUrl(this string url)
        {
            Uri tempUri;
            return Uri.TryCreate(url, UriKind.Absolute, out tempUri);
        }
    }
}