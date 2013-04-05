using System;

namespace IcucApp.Core.Diagnostics
{
    // http://docs.xamarin.com/Android/Guides/Deployment%2C_Testing%2C_and_Metrics/Android_Debug_Log
    public class AndroidLogAppender : ILogAppender
    {
        private readonly string _logTag;

        public AndroidLogAppender(string logTag)
        {
            _logTag = logTag;
        }

        public void Log(LogLevel level, string name, string text)
        {
            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            var message = "{0} [{1}] {2}: {3}".FormatWith(level, threadId, name, text);
            switch (level)
            {
                case LogLevel.Fatal:
                    Android.Util.Log.Wtf(_logTag, message);
                    break;
                case LogLevel.Error:
                    Android.Util.Log.Error(_logTag, message);
                    break;
                case LogLevel.Warn:
                    Android.Util.Log.Warn(_logTag, message);
                    break;
                case LogLevel.Info:
                    Android.Util.Log.Info(_logTag, message);
                    break;
                case LogLevel.Debug:
                    Android.Util.Log.Debug(_logTag, message);
                    break;
            }
        }

        public void Log(LogLevel level, string name, object message, Exception exception)
        {
            var threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            var text = LogManager.SafeFormat("{0} [{1}] {2}: {3}", level, threadId, name, message);
            switch (level)
            {
                case LogLevel.Fatal:
                    Android.Util.Log.Wtf(_logTag, Java.Lang.Throwable.FromException(exception), text);
                    break;
                case LogLevel.Error:
                    Android.Util.Log.Error(_logTag, Java.Lang.Throwable.FromException(exception), text);
                    break;
                case LogLevel.Warn:
                    Android.Util.Log.Warn(_logTag, Java.Lang.Throwable.FromException(exception), text);
                    break;

                case LogLevel.Info:
                case LogLevel.Debug:
                    // no logging with exception for info and debug
                    break;
            }
        }
    }
}