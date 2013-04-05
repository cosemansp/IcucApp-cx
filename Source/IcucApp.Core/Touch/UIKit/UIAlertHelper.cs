using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.UIKit;

namespace IcucApp.Core.Touch.UIKit
{
    public class ButtonAction
    {
        public ButtonAction(string title, Action callback)
        {
            Title = title;
            Callback = callback;
        }

        public string Title { get; set; }
        public Action Callback { get; set; }
    }

    public class UIAlertHelper
    {
        private static readonly List<UIAlertView> ActiveViews = new List<UIAlertView>();
        private static readonly List<UIAlertView> QueuedViews = new List<UIAlertView>();

        public static void ShowAlert(string title, string message, string cancelButtonTitle = null, params ButtonAction[] actions)
        {
            string[] buttons = (actions != null) ?
                actions.Select(x => x.Title).ToArray() : null;

            if (cancelButtonTitle == null)
                cancelButtonTitle = "Cancel";

            var alertView = new UIAlertView(title, message, null, cancelButtonTitle, buttons);
            alertView.Clicked += (sender, e) => HandleAlertViewClicked(alertView, e, actions);
            alertView.Dismissed += HandleAlertViewDismissed;

            if (ActiveViews.Count == 0)
            {
                alertView.Show();
                ActiveViews.Add(alertView);
            }
            else
            {
                QueuedViews.Add(alertView);
            }
        }

        static void HandleAlertViewDismissed(object sender, UIButtonEventArgs e)
        {
            var alertView = (UIAlertView)sender;
            if (ActiveViews.Contains(alertView))
            {
                ActiveViews.Remove(alertView);

                if (QueuedViews.Count > 0)
                {
                    UIAlertView first = QueuedViews.First();
                    first.Show();
                    ActiveViews.Add(first);
                    QueuedViews.Remove(first);
                }
            }
        }

        static void HandleAlertViewClicked(UIAlertView alertView, UIButtonEventArgs e, IEnumerable<ButtonAction> actions)
        {
            if (actions == null)
            {
                return;
            }

            string title = alertView.ButtonTitle(e.ButtonIndex);
            var action = actions.SingleOrDefault(x => x.Title == title);
            if (action != null)
            {
                action.Callback();
            }
        }
    }
}