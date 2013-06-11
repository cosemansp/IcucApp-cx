using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog.Utilities;
using MonoTouch.Foundation;
using IcucApp.Presentation.ViewModels;
using System.Drawing;
using IcucApp.Core.Touch.UIKit;

namespace IcucApp
{
	public class LoadingErrorCell : UITableViewCell
	{
		private readonly UILabel _titleLabel;
        private readonly UILabel _subTitleLabel;
		
		public const int Margin = 8;
		
        public LoadingErrorCell (UITableViewCellStyle style, NSString ident, string title, string subTitle) : base(style, ident)
		{
			SelectionStyle = UITableViewCellSelectionStyle.None;
			
			_titleLabel = new UILabelEx
			{
                Text = title,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName("HelveticaNeue", 15),
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.FromWhiteAlpha(0f, 0f),
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap,
				VerticalAlignment = UILabelEx.VerticalAlignments.Top
			};

            _subTitleLabel = new UILabelEx
            {
                Text = subTitle,
                TextAlignment = UITextAlignment.Center,
                Font = UIFont.FromName("HelveticaNeue", 13),
                TextColor = UIColor.DarkGray,
                BackgroundColor = UIColor.FromWhiteAlpha(0f, 0f),
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                VerticalAlignment = UILabelEx.VerticalAlignments.Top
            };
			
			Update(title, subTitle);

			ContentView.Add(_titleLabel);
            ContentView.Add(_subTitleLabel);
		}
		
		public override void LayoutSubviews ()
		{
			base.LayoutSubviews();
	
            _titleLabel.Frame = new RectangleF(Margin, Margin, 280, 0);
            _titleLabel.SizeToFit();
            _subTitleLabel.Frame = new RectangleF(0, 0, 280, 0);
            _subTitleLabel.SizeToFit();
            _subTitleLabel.CenterBelowSlibling(this, _titleLabel, Margin);
		}
		
		public void Update (string title, string subTitle)
		{
			_titleLabel.Text = title;
            _subTitleLabel.Text = subTitle;
            LayoutSubviews();
		}
		
	}
}

