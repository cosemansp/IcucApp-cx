using System;
using System.Threading;
using IcucApp.Core.Diagnostics;

#if TOUCH
    using MonoTouch.Foundation;
#endif
#if DROID
    using Android.App;
#endif

namespace IcucApp.Core
{
    public interface IDispatcher
    {
        void DispatchOnMainThread(Action action);
        void ExecuteAsync<TResult>(Func<TResult> action, Action<TResult, Exception> finishedCallback = null);
    }

    public class Dispatcher : IDispatcher
    {

#if TOUCH
        private readonly NSObject _owner;

        public Dispatcher(NSObject owner)
        {
            _owner = owner;
        }

        public void DispatchOnMainThread(Action action)
        {
            _owner.BeginInvokeOnMainThread(new NSAction(action));
        }
#endif

#if DROID
        private static readonly ILog _log = LogManager.GetLogger(typeof(Dispatcher));
        private static Activity _owner;
        public static void SetMainActivity(Activity mainActivity, bool overwriteMainAcitivity = false)
        {
            if (_owner == null || overwriteMainAcitivity)
            {
                _log.InfoFormat("SetMainActivity: current owner = {0}, new owner = {1}, isOverwrite: {2}", _owner,  mainActivity, overwriteMainAcitivity);
                _owner = mainActivity;
            }
        }
        public Dispatcher(Activity owner = null)
        {
            _owner = owner;
        }

        public void DispatchOnMainThread(Action action)
        {
            _log.InfoFormat("Dispatcher - TRY - DispatchOnMainThread");
            if (_owner != null)
            {
                _owner.RunOnUiThread(action);
                _log.InfoFormat("Dispatcher - SUCCEED - DispatchOnMainThread");
            }
        }
#endif

        public void ExecuteAsync<TResult>(Func<TResult> action, Action<TResult, Exception> finishedCallback = null)
        {
            Action<object> asyncProcess = obj =>
            {
                try
                {
                    var result = action.Invoke();
                    if (finishedCallback != null)
                    {
                        DispatchOnMainThread(() =>
                        {
                            try
                            {
                                finishedCallback.Invoke(result, null);
                            }
                            catch (Exception exception)
                            {
                                finishedCallback.Invoke(result, exception);
                            }
                        });
                    }
                }
                catch (Exception exception)
                {
                    DispatchOnMainThread(() =>
                    {
                        if (finishedCallback != null)
                        {
                            // pass failure to finished callback
                            finishedCallback.Invoke(default(TResult), exception);
                        }
                        else
                        {
                            // no callback defined, throw exeception in main thread
                            var msg = string.Format("Background task failed: {0}", exception.Message);
                            throw new ApplicationException(msg, exception);
                        }
                    });
                }
            };

            ThreadPool.QueueUserWorkItem(new WaitCallback(asyncProcess));
        }
    }
}