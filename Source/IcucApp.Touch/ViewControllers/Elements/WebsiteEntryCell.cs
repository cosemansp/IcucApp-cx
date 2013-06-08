using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using IcucApp.Presentation.ViewModels;
using IcucApp.Core.Touch.UIKit;
using System.Drawing;
using MonoTouch.Dialog.Utilities;

namespace IcucApp
{
	public class WebsiteEntryCell : UITableViewCell, IImageUpdated 
	{
		private FeedData _data;
		private readonly UIImageView _feedImage;
		private readonly UILabelEx _titleLabel;
		private readonly UILabelEx _descLabel;
		
		public const float ImageWitdh = 60;
		public const float ImageHeigth = (float)(ImageWitdh / 1.0);
		public const int Margin = 8;
		
		public WebsiteEntryCell (UITableViewCellStyle style, NSString ident, FeedData data) : base(style, ident)
		{
			_data = data;
			SelectionStyle = UITableViewCellSelectionStyle.None;
			
			_feedImage = new UIImageView();
			_titleLabel = new UILabelEx
			{
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("HelveticaNeue-Bold", 16),
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.FromWhiteAlpha(0f, 0f),
				Lines = 0,
				LineBreakMode = UILineBreakMode.TailTruncation,
				VerticalAlignment = UILabelEx.VerticalAlignments.Top
			};
			_descLabel = new UILabelEx
			{
				TextAlignment = UITextAlignment.Left,
				Font = UIFont.FromName("HelveticaNeue", 14),
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.FromWhiteAlpha(0f, 0f),
				Lines = 0,
				LineBreakMode = UILineBreakMode.TailTruncation,
				VerticalAlignment = UILabelEx.VerticalAlignments.Top
			};
			
			Update(data);
			
			ContentView.Add(_feedImage);
			ContentView.Add(_titleLabel);
			ContentView.Add(_descLabel);
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews();
			
			_feedImage.Frame = new RectangleF(Margin, Margin, ImageWitdh, ImageHeigth);
			
			// titleLabel 
			_titleLabel.Frame = new RectangleF(0, 0, this.Frame.Width - (Margin*2) - ImageWitdh - Margin - 20, 0);
			_titleLabel.SizeToFit();
			_titleLabel.SnapAnchorPoint(AnchorPoint.TopLeft, new PointF(Margin, -2), AnchorPoint.TopRight, _feedImage)
					   .LimitHeigth(45) /* 2 lines */;

			// descLabel 
			_descLabel.Frame = new RectangleF(0, 0, this.Frame.Width - (Margin*2) - ImageWitdh - Margin - 20, 0);
			_descLabel.SizeToFit();
			_descLabel.LeftAlignBelowSlibling(_titleLabel, Margin / 2)
	   				   .LimitHeigth(ImageHeigth + (2 * Margin) + 20 - _titleLabel.Frame.Height - Margin) /* 2 lines */;
		}
		
		public void Update (FeedData data)
		{
			_data = data;
			_titleLabel.Text = data.Title;
			_descLabel.Text = data.Excerpt;
			if (data.ImageUrl != null) {
				var image = ImageLoader.DefaultRequestImage(data.ImageUrl, this);
				if (image != null) {
					_feedImage.Image = image.Crop(ImageWitdh, ImageHeigth, CropPosition.Center).RemoveSharpEdges(4);
				}
			}
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

