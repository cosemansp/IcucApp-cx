using System;
using System.Threading;
using IcucApp.Core;
using MonoTouch.Foundation;

namespace IcucApp.Touch.Core
{
    public class DispatcherTouch : Dispatcher
    {
        private readonly NSObject _owner;

        public DispatcherTouch(NSObject owner)
        {
            _owner = owner;
        }

        public override void DispatchOnMainThread(Action action)
        {
            _owner.BeginInvokeOnMainThread(new NSAction(action));
        }
    }
}