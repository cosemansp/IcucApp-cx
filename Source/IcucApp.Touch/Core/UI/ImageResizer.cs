using System;
using MonoTouch.UIKit;
using System.Drawing;
using IcucApp.Core;
using MonoTouch.CoreGraphics;

namespace IcucApp
{
	public enum CropPosition
	{
		Start, Center, End
	}
	
	/// <summary>
	/// See http://dantes-andreea.blogspot.be/2011/08/monotouch-class-for-resize-maintaining.html
	/// </summary>
	public class ImageResizer
	{
		public ImageResizer(UIImage image)
		{
			InitialImage = image;
			ModifiedImage = image;
		}
		
		public void RemoveSharpEdges(float radius)
		{
			if (ModifiedImage == null) 
				return;
			
			UIGraphics.BeginImageContext (new SizeF (ModifiedImage.Size.Width, ModifiedImage.Size.Height));
			var context = UIGraphics.GetCurrentContext ();
			if (context != null)
			{
				radius = Display.IsHighRes ? radius * 2 : radius;
				
				const float minx = 0;
				float midx = ModifiedImage.Size.Width / 2;
				float maxx = ModifiedImage.Size.Width;
				float miny = 0;
				float midy = ModifiedImage.Size.Height / 2;
				float maxy = ModifiedImage.Size.Height;
				
				//var path = new CGPath ();
				context.BeginPath ();
				context.MoveTo (minx, midy);
				context.AddArcToPoint (0, miny, midx, miny, radius);
				context.AddArcToPoint (maxx, miny, maxx, midy, radius);
				context.AddArcToPoint (maxx, maxy, midx, maxy, radius);
				context.AddArcToPoint (minx, maxy, minx, midy, radius);		
				context.ClosePath ();
				context.Clip ();
				
				ModifiedImage.Draw (new PointF (0, 0));
				var converted = UIGraphics.GetImageFromCurrentImageContext ();
				ModifiedImage = converted;
			}
			
			UIGraphics.EndImageContext();
		}
		
		/// <summary>
		/// strech resize
		/// </summary>
		public void Resize(float width, float height)
		{
			if (ModifiedImage == null) 
				return;
			
			//if (Display.IsHighRes)
			//{
			//    width = width*2;
			//    height = height*2;
			//}
			
			UIGraphics.BeginImageContext(new SizeF(width, height));
			//
			ModifiedImage.Draw(new RectangleF(0, 0, width, height));
			ModifiedImage = UIGraphics.GetImageFromCurrentImageContext();
			//
			UIGraphics.EndImageContext();
		}
		
		public void RatioResizeToWidth(float width)
		{
			if (ModifiedImage == null) 
				return;
			
			//if (Display.IsHighRes)
			//{
			//    width = width * 2;
			//}
			
			var curWidth = ModifiedImage.Size.Width;
			if (curWidth > width)
			{
				var ratio = width / curWidth;
				var height = ModifiedImage.Size.Height * ratio;
				
				UIGraphics.BeginImageContext(new SizeF(width, height));
				//
				ModifiedImage.Draw(new RectangleF(0, 0, width, height));
				ModifiedImage = UIGraphics.GetImageFromCurrentImageContext();
				//
				UIGraphics.EndImageContext();
			}
		}
		
		public void RatioResizeToHeight(float height)
		{
			if (ModifiedImage == null) 
				return;
			
			//if (Display.IsHighRes)
			//{
			//    height = height * 2;
			//}
			
			var curHeight = ModifiedImage.Size.Height;
			if (curHeight > height)
			{
				var ratio = height / curHeight;
				var width = ModifiedImage.Size.Width * ratio;
				Resize(width, height);
			}
		}
		
		/// <summary>
		/// resize maintaining ratio
		/// </summary>
		public void RatioResize(float maxWidth, float maxHeight)
		{
			//if (Display.IsHighRes)
			//{
			//    maxWidth = maxWidth * 2;
			//    maxHeight = maxHeight * 2;
			//}
			
			if (maxWidth > maxHeight)
			{
				RatioResizeToWidth(maxWidth);
				RatioResizeToHeight(maxHeight);
			}
			else
			{
				RatioResizeToHeight(maxHeight);
				RatioResizeToWidth(maxWidth);
			}
		}
		
		/// <summary>
		/// scaling the image with a given procent value
		/// </summary>
		public void Scale(float procent)
		{
			if (ModifiedImage == null) 
				return;
			
			var width = ModifiedImage.Size.Width * (procent / 100);
			var height = ModifiedImage.Size.Height * (procent / 100);
			Resize(width, height);
		}
		
		/// <summary>
		/// default crop resize, set on center
		/// </summary>
		public void CropResize(float width, float height)
		{
			CropResize(width, height, CropPosition.Center);
		}
		
		/// <summary>
		/// resize and crop to a specific location
		/// </summary>
		public void CropResize(float width, float height, CropPosition position)
		{
			if (ModifiedImage == null) 
				return;
			
			if (Display.IsHighRes)
			{
			    width = width * 2;
			    height = height * 2;
			}
			
			SizeF imgSize = ModifiedImage.Size;
			//
			if (imgSize.Width < width) width = imgSize.Width;
			if (imgSize.Height < height) height = imgSize.Height;
			//
			float cropX = 0;
			float cropY = 0;
			if (imgSize.Width / width < imgSize.Height / height)
			{
				//scad din width
				RatioResizeToWidth(width);
				imgSize = ModifiedImage.Size;
				//compute crop_y
				if (position == CropPosition.Center) cropY = (imgSize.Height / 2) - (height / 2);
				if (position == CropPosition.End) cropY = imgSize.Height - height;
			}
			else
			{
				//change height
				RatioResizeToHeight(height);
				imgSize = ModifiedImage.Size;
				//calculeaza crop_x
				if (position == CropPosition.Center) cropX = (imgSize.Width / 2) - (width / 2);
				if (position == CropPosition.End) cropX = imgSize.Width - width;
			}
			//create new contect
			UIGraphics.BeginImageContext(new SizeF(width, height));
			CGContext context = UIGraphics.GetCurrentContext();
			if (context != null)
			{
				//crops the new context to the desired height and width
				var clippedRect = new RectangleF(0, 0, width, height);
				context.ClipToRect(clippedRect);
				//draw my image on the context
				var drawRect = new RectangleF(-cropX, -cropY, imgSize.Width, imgSize.Height);
				ModifiedImage.Draw(drawRect);
				//save the context in modifiedImage
				ModifiedImage = UIGraphics.GetImageFromCurrentImageContext();
			}
			//close the context
			UIGraphics.EndImageContext();
			
		}
		
		
		public UIImage InitialImage { get; private set; }
		
		public UIImage ModifiedImage { get; private set; }
	}
}

