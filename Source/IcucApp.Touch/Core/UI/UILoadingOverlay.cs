using System.Drawing;
using MonoTouch.UIKit;

namespace IcucApp.Core.Touch.UIKit
{
    public class UILoadingOverlay : UIView {

        readonly UIActivityIndicatorView _activitySpinner;
        readonly UILabel _infoLabel;

        public UILoadingOverlay(RectangleF frame)
            : base(frame)
        {
            // configurable bits
            BackgroundColor = UIColor.White;
            Alpha = 0.10f;
            AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

            // derive the center x and y
            float centerX = Frame.Width / 2;
            float centerY = Frame.Height / 2;

            // create the activity spinner, center it horizontall and put it 5 points above center x
            _activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Gray);
            _activitySpinner.Frame = new RectangleF(centerX - (_activitySpinner.Frame.Width / 2),
                                                    centerY - _activitySpinner.Frame.Height - 20,
                                                    _activitySpinner.Frame.Width,
                                                    _activitySpinner.Frame.Height);
            _activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
            AddSubview(_activitySpinner);
            _activitySpinner.StartAnimating();

            // create and configure the info label
            _infoLabel = new UILabel();
            _infoLabel.BackgroundColor = UIColor.Clear;
            _infoLabel.TextColor = UIColor.DarkGray;
            _infoLabel.Font = UIFont.FromName("HelveticaNeue", 14);
            _infoLabel.TextAlignment = UITextAlignment.Center;
            _infoLabel.Lines = 0;
            AddSubview(_infoLabel);
        }

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			_infoLabel.SizeToFit();
		    _infoLabel.CenterAndStretchBelowSlibling(this, _activitySpinner, 20);
		}

        public void SetInfoText(string text)
        {
            _infoLabel.Text = text;
			LayoutSubviews();
        }

        public void ShowAnimation(bool show)
        {
            _activitySpinner.Hidden = !show;
        }

        /// <summary>
        /// Fades out the control and then removes it from the super view
        /// </summary>
        public void Hide(bool animated = true)
        {
            if (animated)
            {
                Animate(0.5, () => { Alpha = 0; },  RemoveFromSuperview);
            }
            else
            {
                RemoveFromSuperview();
            }
        }
    }
}