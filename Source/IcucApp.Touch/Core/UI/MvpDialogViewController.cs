using System;
using System.Drawing;
using IcucApp.Core.Ioc;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace IcucApp.Core.UI
{

    public class CustomRefreshTableHeaderView : RefreshTableHeaderView
    {
        public CustomRefreshTableHeaderView(RectangleF rect)
            : base(rect)
        {
            BackgroundColor = UIColor.White;
            LastUpdateLabel.BackgroundColor = BackgroundColor;
            StatusLabel.BackgroundColor = BackgroundColor;
            StatusLabel.Font = UIFont.SystemFontOfSize(12f);
            StatusLabel.TextColor = UIColor.Black;
            LastUpdateLabel.TextColor = UIColor.Black;
        }
    }

    public class MvpDialogViewController<TPresenter> : DialogViewController where TPresenter : class, IPresenter
    {
        private readonly TPresenter _presenter;
        private UIActivityIndicatorView _activitySpinner;

        public MvpDialogViewController(UITableViewStyle style, bool pushing = true)
            : base(style, null, pushing)
        {
            _presenter = Container.Resolve<TPresenter>(new NamedParameter() { { "view", this } });
        }

        protected void EnableRefresh()
        {
            if (Device.SystemVersion.Major >= 6)
            {
                // iOS6 and higher
                RefreshControl = new UIRefreshControl();
                RefreshControl.ValueChanged += OnPullDownRefresh;
            }
            else
            {
                // iOS5
                RefreshRequested += OnPullDownRefresh;
            }
        }

        protected virtual void OnPullDownRefresh(object sender, EventArgs e)
        {
        }

        protected void ShowSpinner(bool show = true)
        {
            if (!show) {
                if (_activitySpinner != null) {
                    _activitySpinner.Hidden = true;
                    _activitySpinner.RemoveFromSuperview();
                    _activitySpinner = null;
                }
                return;
            }

            if (_activitySpinner == null) {
                // derive the center x and y
                float centerX = View.Frame.Width / 2;
                float centerY = View.Frame.Height / 2;

                _activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
                _activitySpinner.Frame = new RectangleF(centerX - (_activitySpinner.Frame.Width / 2),
                                                        centerY - _activitySpinner.Frame.Height - 20,
                                                        _activitySpinner.Frame.Width,
                                                        _activitySpinner.Frame.Height);
                _activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
                View.AddSubview(_activitySpinner);
            }
            _activitySpinner.StartAnimating();
        }

        protected void EndRefreshing()
        {
            if (Device.SystemVersion.Major >= 6)
            {
                // IOS6 and higher
                RefreshControl.EndRefreshing();
            }
            // iOS5
            ReloadComplete();
        }

        public TPresenter Presenter
        {
            get { return _presenter; }
        }
    }
}