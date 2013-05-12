using System;
using MonoTouch.UIKit;

namespace IcucApp
{
	public static class ImageExtensions
	{
		public static UIImage RemoveSharpEdges(this UIImage image, float radius)
		{
			var imageSizer = new ImageResizer(image);
			imageSizer.RemoveSharpEdges(radius);
			return imageSizer.ModifiedImage;
		}
		
		public static UIImage RatioResizeToWidth(this UIImage image, float width)
		{
			var imageSizer = new ImageResizer(image);
			imageSizer.RatioResizeToWidth(width);
			return imageSizer.ModifiedImage;
		}
		
		public static UIImage Scale(this UIImage image, float procent)
		{
			var imageSizer = new ImageResizer(image);
			imageSizer.Scale(procent);
			return imageSizer.ModifiedImage;
		}
		
		public static UIImage Resize(this UIImage image, float width, float height)
		{
			var imageSizer = new ImageResizer(image);
			imageSizer.Resize(width, height);
			return imageSizer.ModifiedImage;
		}
		
		public static UIImage Crop(this UIImage image, float width, float height, CropPosition position = CropPosition.Center)
		{
			var imageSizer = new ImageResizer(image);
			imageSizer.CropResize(width, height, position);
			return imageSizer.ModifiedImage;
		}
	}
}

