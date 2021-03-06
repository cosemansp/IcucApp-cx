using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using IcucApp.Presentation.ViewModels;
using IcucApp.Core.UI;
using System.Drawing;
using IcucApp.Core.Touch.UIKit;
using System.Globalization;

namespace IcucApp
{
	public class LineupEntryCell : UITableViewCell, IImageUpdated 
	{
		private FeedData _data;
		private readonly UIImageView _feedImage;
		private readonly UILabel _titleLabel;
        private UILabel _timeLabel;

		public const float ImageWitdh = 60;
		public const float ImageHeigth = (float)(ImageWitdh / 1.0);
		public const int Margin = 8;

		public LineupEntryCell (UITableViewCellStyle style, NSString ident, FeedData data) : base(style, ident)
		{
			_data = data;
			SelectionStyle = UITableViewCellSelectionStyle.None;

			_feedImage = new UIImageView();
			_titleLabel = new UILabel
			{
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("HelveticaNeue-Bold", 20),
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.FromWhiteAlpha(0f, 0f)
			};
            _timeLabel = new UILabel
            {
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.FromName("HelveticaNeue", 14),
                TextColor = UIColor.Gray,
                BackgroundColor = UIColor.FromWhiteAlpha(0f, 0f),
            };

			Update(data);
			
			ContentView.Add(_feedImage);
			ContentView.Add(_titleLabel);
            ContentView.Add(_timeLabel);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews();

			_feedImage.Frame = new RectangleF(Margin, Margin, ImageWitdh, ImageHeigth);

			// titleLabel 
			_titleLabel.Frame = new RectangleF(0, 0, this.Frame.Width - (Margin*2) - ImageWitdh - Margin, 0);
			_titleLabel.SizeToFit();
			_titleLabel.SnapAnchorPoint(AnchorPoint.TopLeft, new PointF(Margin, -2), AnchorPoint.TopRight, _feedImage)
				.LimitHeigth(60) /* 3 lines */;
            _timeLabel.SizeToFit();
            _timeLabel.LeftAlignBelowSlibling(_titleLabel, 0);
		}

		public void Update (FeedData data)
		{
			_data = data;
			_titleLabel.Text = data.Title;
			if (data.ImageUrl != null) {
				var image = ImageLoader.DefaultRequestImage(data.ImageUrl, this);
				if (image != null) {
					_feedImage.Image = image.Crop(ImageWitdh, ImageHeigth, CropPosition.Center).RemoveSharpEdges(4);
				}
                else {
                    _feedImage.Image = null;
                }
			}
            _timeLabel.Text = data.Published.ToString("HH:mm", new CultureInfo("nl-BE"));
		}

		public void UpdatedImage(Uri uri)
		{
			if (_data.ImageUrl == null)
				return;
			
			if (_data.ImageUrl == uri)
			{
				var image = ImageLoader.DefaultRequestImage(uri, this);
				if (image != null) {
					_feedImage.Image = image.Crop(ImageWitdh, ImageHeigth, CropPosition.Center).RemoveSharpEdges(4);
				}
			}
		}
	}
}

