#if TOUCH
using MonoTouch.Foundation;
using MonoTouch.UIKit;

#endif

#if DROID
using Android.App;
using Android.Content;
using Android.Net;
#endif

namespace IcucApp.Core.Services
{
    public interface IWebBrowser
    {
        void OpenUrl(string url);
    }

    public class WebBrowser : IWebBrowser
    {
        public void OpenUrl(string url)
        {
#if DROID
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "http://" + url;

            var browserIntent = new Intent(Intent.ActionView, Uri.Parse(url));
            browserIntent.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(browserIntent);
#elif TOUCH
            UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
#else
#endif
        }
    }
}