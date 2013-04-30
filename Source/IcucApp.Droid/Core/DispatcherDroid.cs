using System;
using Android.App;

namespace IcucApp.Core
{
    public class DispatcherDroid : Dispatcher
    {
        private static Activity _owner;
        public static void SetMainActivity(Activity mainActivity, bool overwriteMainAcitivity = false)
        {
            if (_owner == null || overwriteMainAcitivity)
            {
                _owner = mainActivity;
            }
        }
        public DispatcherDroid(Activity owner = null)
        {
            _owner = owner;
        }

        public override void DispatchOnMainThread(Action action)
        {
            if (_owner != null)
            {
                _owner.RunOnUiThread(action);
            }
        }
    }
}