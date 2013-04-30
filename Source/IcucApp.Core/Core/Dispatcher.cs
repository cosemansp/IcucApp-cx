using System;
using System.Threading;

namespace IcucApp.Core
{
    public interface IDispatcher
    {
        void DispatchOnMainThread(Action action);
        void ExecuteAsync<TResult>(Func<TResult> action, Action<TResult, Exception> finishedCallback = null);
    }

    public abstract class Dispatcher : IDispatcher
    {
        public abstract void DispatchOnMainThread(Action action);

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