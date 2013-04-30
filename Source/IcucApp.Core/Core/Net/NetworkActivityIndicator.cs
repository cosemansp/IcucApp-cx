using System;

#if TOUCH
using MonoTouch.UIKit;
#endif

namespace IcucApp.Core.Net
{

    public static class NetworkActivityIndicator
    {
#if TOUCH
        private static int _refCounter;
        private static readonly object SyncLock = new object();

        static long _lastLaunchTick;
        static readonly object MinuteLock = new object();
#elif DROID
        // todo: implement for Android
#endif

        public static void ShowActivity()
        {
#if TOUCH
            lock (SyncLock)
            {
                _refCounter++;
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            }
#elif DROID
            // todo: implement for Android
#endif
        }

        public static void Wakeup3G(Uri uri)
        {
#if TOUCH
            // Wake up 3G if it has been more than 3 minutes
            lock (MinuteLock)
            {
                if (uri != null)
                {
                    var nowTicks = DateTime.UtcNow.Ticks;
                    if (nowTicks - _lastLaunchTick > TimeSpan.TicksPerMinute * 3)
                        MonoTouch.ObjCRuntime.Runtime.StartWWAN(uri);
                    _lastLaunchTick = nowTicks;
                }
            }
#elif DROID
            // not required on Android
#endif
        }
         
        public static void HideActivity()
        {
#if TOUCH
            lock (SyncLock)
            {
                _refCounter--;
                if (_refCounter <= 0)
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            }
#elif DROID
            // todo: implement for Android
#endif
        }
    }
}