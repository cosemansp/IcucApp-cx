using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace IcucApp.Core.Touch.UIKit
{
	public class UILabelEx: UILabel {
		
		public enum VerticalAlignments
		{
			Middle = 0, //the default (what standard UILabels do) 
			Top,
			Bottom
		}
		
		private VerticalAlignments _verticalAlignment;
		public VerticalAlignments VerticalAlignment
		{
			get { return _verticalAlignment; }
			set
			{
				if (_verticalAlignment != value)
				{
					_verticalAlignment = value;
					SetNeedsDisplay();    //redraw if value changed 
				}
			}
		} 
		
		public UILabelEx()
		{
			
		}
		
		public UILabelEx(RectangleF rect) : base(rect)
		{
			
		}
		
		public override void DrawText(RectangleF rect)
		{
			//normally it uses full size of the control - we change this 
			RectangleF rErg = TextRectForBounds(rect, Lines);
			base.DrawText(rErg);
		}
		
		//calculate the rect for text output - depending on VerticalAlignment 
		public override RectangleF TextRectForBounds(RectangleF bounds, int numberOfLines)
		{
			RectangleF calculatedRect = base.TextRectForBounds(bounds, numberOfLines);
			if (_verticalAlignment != VerticalAlignments.Top)
			{    
				//no special handling for top
				if (_verticalAlignment == VerticalAlignments.Bottom)
				{
					bounds.Y += bounds.Height - calculatedRect.Height;    //move down by difference
				}
				else
				{    //middle == nothing set == somenthing strange ==> act like standard UILabel
					bounds.Y += (bounds.Height - calculatedRect.Height) / 2;
				}
			}
			bounds.Height = calculatedRect.Height;    //always the calculated height 
			return (bounds);
		} 
	}
}

