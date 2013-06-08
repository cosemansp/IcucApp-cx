using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace IcucApp.Core.Touch.UIKit
{
	public enum AnchorPoint
	{
		TopLeft = 1 << 0,
		Top = 1 << 1,
		TopRight = 1 << 2,
		Left = 1 << 3,
		Center = 1 << 4,
		Right = 1 << 5,
		BottomLeft = 1 << 6,
		Bottom = 1 << 7,
		BottomRight = 1 << 8
	}
	
	// http://jayfuerstenberg.com/devblog/tag/design
	
	public static class UIViewExtensions
	{
		/// <summary>
		/// Vertically centers the provided view at x within the superview
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to vertically center</param>
		/// <param name="superView">The super view within which the view will be vertically centered.</param>
		/// <param name="x">The horizontal offset from which the view will be vertically centered.</param>
		/// <returns>The centered view</returns>
		public static T VerticallyCenterAtX<T>(this T view, UIView superView, float x) where T : UIView
		{
			float superViewCenter = superView.Bounds.Size.Height / 2.0f;
			float viewCenter = view.Bounds.Size.Height / 2.0f;
			float y = superViewCenter - viewCenter;
			var newFrame = new RectangleF(x, y, view.Frame.Size.Width, view.Frame.Size.Height);
			view.Frame = newFrame;
			return view;
		}
		
		/// <summary>
		/// Centers the provided view at y within the superview.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center</param>
		/// <param name="superView">The super view within which the view will be centered.</param>
		/// <param name="y">The vertical offset from which the view will be centered</param>
		/// <returns>The centered view</returns>
		public static T CenterAtY<T>(this T view, UIView superView, float y) where T : UIView
		{
			float superViewCenter = superView.Bounds.Size.Width / 2.0f;
			float viewCenter = view.Bounds.Size.Width / 2.0f;
			float x = superViewCenter - viewCenter;
			var newFrame = new RectangleF(x, y, view.Frame.Size.Width, view.Frame.Size.Height);
			view.Frame = newFrame;
			return view;
		}
		
		/// <summary>
		/// Centers and stretch the provided view within the superview.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center</param>
		/// <param name="superView">The super view within which the view will be centered.</param>
		/// <param name="margin">The margin within the superview</param>
		/// <returns>The centered view</returns>
		public static T CenterAndStretch<T>(this T view, UIView superView, float margin) where T : UIView
		{
			// set view width
			var width = superView.Frame.Width - (margin * 2);
			var viewFrame = superView.Frame;
			viewFrame.Width -= (margin*2);
			view.Frame = viewFrame;
			view.SizeToFit();
			
			//float superViewCenter = superView.Bounds.Size.Width / 2.0f;
			//float viewCenter = view.Bounds.Size.Width / 2.0f;
			//float x = superViewCenter - viewCenter;
			
			var newFrame = new RectangleF(margin, margin, width, view.Frame.Size.Height);
			view.Frame = newFrame;
			return view;
		}
		
		/// <summary>
		/// Centers and stretch the provided view within the superview and below the sibling view
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center</param>
		/// <param name="superView">The super view within which the view will be centered.</param>
		/// <param name="siblingView">The sibling view under which the view will appear</param>
		/// <param name="margin">The margin within the superview</param>
		/// <returns>The centered view</returns>
		public static T CenterAndStretchBelowSlibling<T>(this T view, UIView superView, UIView siblingView, float margin) where T : UIView
		{
			var viewFrame = superView.Frame;
			viewFrame.Width -= (margin * 2);
			view.Frame = viewFrame;
			view.SizeToFit();
			view.CenterBelowSlibling(superView, siblingView, margin);
			return view;
		}
		
		/// <summary>
		/// Centers the provided view within the superview.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center</param>
		/// <param name="superView">The super view within which the view will be centered.</param>
		/// <returns>The centered view</returns>
		public static T CenterView<T>(this T view, UIView superView) where T : UIView
		{
			float superViewCenterX = superView.Bounds.Size.Width / 2.0f;
			float viewCenterX = view.Bounds.Size.Width / 2.0f;
			float x = superViewCenterX - viewCenterX;
			float superViewCenterY = superView.Bounds.Size.Height / 2.0f;
			float viewCenterY = view.Bounds.Size.Height / 2.0f;
			float y = superViewCenterY - viewCenterY;
			var newFrame = new RectangleF(x, y, view.Frame.Size.Width, view.Frame.Size.Height);
			view.Frame = newFrame;
			return view;
		}
		
		/// <summary>
		/// Centers the provided view within the super view and below a sibling distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center</param>
		/// <param name="superView">The super view within which the view will be centered.</param>
		/// <param name="siblingView">The sibling view under which the view will appear</param>
		/// <param name="margin">The margin/offset between the view and the sibling view.</param>
		/// <returns>The centered view</returns>
		public static T CenterBelowSlibling<T>(this T view, UIView superView, UIView siblingView, float margin) where T : UIView
		{
			float superViewCenter = superView.Bounds.Size.Width / 2.0f;
			float siblingCenter = siblingView.Frame.X + (siblingView.Frame.Size.Width / 2.0f);
			var marginPoint = new PointF(superViewCenter - siblingCenter, margin);
			SnapAnchorPoint(view, AnchorPoint.Top, marginPoint, AnchorPoint.Bottom, siblingView);
			return view;
		}
		
		/// <summary>
		/// Centers the provided view below a sibling view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center.</param>
		/// <param name="siblingView">The sibling view under which the view will appear.</param>
		/// <param name="margin">The margin/offset between the view and the sibling view.</param>
		/// <returns></returns>
		public static T CenterBelowSlibling<T>(this T view, UIView siblingView, float margin) where T : UIView
		{
			var marginPoint = new PointF(0.0f, margin);
			view.SnapAnchorPoint(AnchorPoint.Top, marginPoint, AnchorPoint.Bottom, siblingView);
			return view;
		}
		
		/// <summary>
		/// Vertically centers the provided view to the left of a sibling view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center.</param>
		/// <param name="siblingView">The sibling view to the left of which the view will appear.</param>
		/// <param name="margin">The margin/offset between the view and the sibling view.</param>
		/// <returns></returns>
		public static T VerticalCenterLeftOfSlibling<T>(this T view, UIView siblingView, float margin) where T : UIView
		{
			var marginPoint = new PointF(-margin, 0.0f);
			view.SnapAnchorPoint(AnchorPoint.Right, marginPoint, AnchorPoint.Left, siblingView);
			return view;
		}
		
		/// <summary>
		/// Vertically centers the provided view to the right of a sibling view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to center.</param>
		/// <param name="siblingView">The sibling view to the left of which the view will appear.</param>
		/// <param name="margin">The margin/offset between the view and the sibling view.</param>
		/// <returns></returns>
		public static T VerticalCenterRightOfSlibling<T>(this T view, UIView siblingView, float margin) where T : UIView
		{
			var marginPoint = new PointF(-margin, 0.0f);
			view.SnapAnchorPoint(AnchorPoint.Left, marginPoint, AnchorPoint.Right, siblingView);
			return view;
		}
		
		/// <summary>
		/// Left aligns the provided view below a sibling view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to left align</param>
		/// <param name="siblingView">The sibling view under which the view will appear.</param>
		/// <param name="margin">The margin/offset between the view and the sibling view.</param>
		/// <returns></returns>
		public static T LeftAlignBelowSlibling<T>(this T view, UIView siblingView, float margin) where T : UIView
		{
			var marginPoint = new PointF(0.0f, margin);
			view.SnapAnchorPoint(AnchorPoint.TopLeft, marginPoint, AnchorPoint.BottomLeft, siblingView);
			return view;
		}
		
		/// <summary>
		/// Left aligns the provided view to the sibling view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to left align</param>
		/// <param name="siblingView">The sibling view under which the view will appear.</param>
		/// <param name="margin">The margin/offset between the view and the sibling view.</param>
		/// <returns></returns>
		public static T LeftAlign<T>(this T view, UIView siblingView, float margin) where T : UIView
		{
			var marginPoint = new PointF(margin, 0.0f);
			view.SnapAnchorPoint(AnchorPoint.Left, marginPoint, AnchorPoint.Right, siblingView);
			return view;
		}
		
		/// <summary>
		/// Left aligns the provided view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to left align</param>
		/// <param name="superView">The sibling view under which the view will appear.</param>
		/// <param name="marginPoint">The margin/offset between the view and superview.</param>
		/// <returns></returns>
		public static T LeftAlignSuperview<T>(this T view, UIView superView, PointF marginPoint) where T : UIView
		{
			view.Frame = new RectangleF(0 + marginPoint.X, 0 + marginPoint.Y, view.Frame.Width, view.Frame.Height);
			return view;
		}
		
		/// <summary>
		/// Rigth aligns the provided view below a sibling view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to right align</param>
		/// <param name="siblingView">The sibling view under which the view will appear.</param>
		/// <param name="margin">The margin/offset between the view and the sibling view.</param>
		/// <returns>The view</returns>
		public static T RightAlignBelowSlibling<T>(this T view, UIView siblingView, float margin) where T : UIView
		{
			var marginPoint = new PointF(0.0f, margin);
			view.SnapAnchorPoint(AnchorPoint.TopRight, marginPoint, AnchorPoint.BottomRight, siblingView);
			return view;
		}
		
		/// <summary>
		/// Limit the height of the view
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to limit the height</param>
		/// <param name="maxHeight">The maximum height</param>
		/// <returns>The view</returns>
		public static T LimitHeigth<T>(this T view, float maxHeight) where T : UIView
		{
			if (view.Frame.Height > maxHeight)
				view.Frame = new RectangleF(view.Frame.X, view.Frame.Y, view.Frame.Width, maxHeight);
			return view;
		}
		
		/// <summary>
		/// Limit the width of the view
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to limit the width</param>
		/// <param name="maxWidth">The maximum width</param>
		/// <returns>The view</returns>
		public static T LimitWidth<T>(this T view, float maxWidth) where T : UIView
		{
			if (view.Frame.Width > maxWidth)
				view.Frame = new RectangleF(view.Frame.X, view.Frame.Y, maxWidth, view.Frame.Height);
			return view;
		}
		
		/// <summary>
		/// Rigth aligns the provided view to the super view distanced by the margin/offset.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to right align</param>
		/// <param name="superView">The sibling view under which the view will appear.</param>
		/// <param name="marginPoint">The margin/offset between the view and the sibling view.</param>
		/// <returns></returns>
		public static T RightAlignSuperview<T>(this T view, UIView superView, PointF marginPoint) where T : UIView
		{
			view.Frame = new RectangleF(superView.Frame.Width - marginPoint.X - view.Frame.Width, 0 + marginPoint.Y, view.Frame.Width, view.Frame.Height);
			return view;
		}
		
		/// <summary>
		/// Snaps the provided view by the anchor point to the sibling view's anchor point.
		/// If either view is nil the method will perform no anchoring.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to anchor</param>
		/// <param name="anchorPoint">The view's anchor point</param>
		/// <param name="margin"></param>
		/// <param name="siblingAnchorPoint">The sibling view's anchor point.</param>
		/// <param name="siblingView">The sibling view.</param>
		/// <returns></returns>
		public static T SnapAnchorPoint<T>(this T view, AnchorPoint anchorPoint, PointF margin, AnchorPoint siblingAnchorPoint, UIView siblingView) where T : UIView
		{
			if (view == null || siblingView == null)
			{
				return view;
			}
			
			PointF siblingPoint = PointForAnchor(siblingView, siblingAnchorPoint);
			PointF point = PointForAnchor(view, anchorPoint);
			MoveView(view, point, siblingPoint, margin);
			return view;
		}
		
		/// <summary>
		/// Snaps the provided view by the anchor point to the sibling view's anchor point.
		/// If either view is nil the method will perform no anchoring.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="view">The view to anchor</param>
		/// <param name="anchorPoint">The view's anchor point</param>
		/// <param name="siblingAnchorPoint">The sibling view's anchor point.</param>
		/// <param name="siblingView">The sibling view.</param>
		/// <returns></returns>
		public static T SnapAnchorPoint<T>(this T view, AnchorPoint anchorPoint, AnchorPoint siblingAnchorPoint, UIView siblingView) where T : UIView
		{
			if (view == null || siblingView == null)
			{
				return view;
			}
			
			PointF siblingPoint = PointForAnchor(siblingView, siblingAnchorPoint);
			PointF point = PointForAnchor(view, anchorPoint);
			MoveView(view, point, siblingPoint);
			return view;
		}
		
		/// <summary>
		/// Sets the height of the provided view to fit all its subviews and adding the margin.
		/// </summary>
		/// <param name="superview">The view to resize.</param>
		/// <param name="margin">The margin to add to the eventual height.</param>
		public static void SetHeightToFitAllSubviews(this UIView superview, float margin)
		{
			float deepestY = 0.0f;
			var subviews = superview.Subviews;
			if (subviews.Length == 0)
				return; // no subviews, no action
			
			// get deepest Y
			foreach (var subview in subviews)
			{
				if (subview.Hidden)
					continue;
				float y = subview.Frame.Y + subview.Frame.Size.Height;
				if (y > deepestY)
				{
					deepestY = y;
				}
			}
			
			// set superview height
			var currentFrame = superview.Frame;
			var newFrame = new RectangleF(currentFrame.X, currentFrame.Y, currentFrame.Size.Width, deepestY + margin);
			superview.Frame = newFrame;
			
			// set contentSize if the superview is a scroll view
			if (superview is UIScrollView) {
				var scrollView = (UIScrollView) superview;
				scrollView.ContentSize = newFrame.Size;
			}
		}
		
		/// <summary>
		/// Snaps the provided view to neatly discreet coordinates.
		/// </summary>
		/// <param name="view">The view whose coordinates will be snapped.</param>
		public static void SnapToDiscreteCoordinates(this UIView view)
		{
			if (view == null)
				return;
			
			var point = PointForAnchor(view, AnchorPoint.TopLeft);
			point.X = (float)Math.Round(point.X);
			point.Y = (float)Math.Round(point.Y);
			
			view.Frame = new RectangleF(point.X, point.Y, view.Frame.Size.Width, view.Frame.Size.Height);
		}
		
		public static T MoveView<T>(this T view, PointF point) where T : UIView
		{
			var frame = view.Frame;
			var newFrame = new RectangleF(frame.X = point.X, frame.Y = point.Y, frame.Size.Width, frame.Size.Height);
			view.Frame = newFrame;
			return view;
		}
		
		public static T MoveView<T>(this T view, PointF point, PointF otherPoint) where T : UIView
		{
			float xDiff = otherPoint.X - point.X;
			float yDiff = otherPoint.Y - point.Y;
			var frame = view.Frame;
			var newFrame = new RectangleF(frame.X += xDiff, frame.Y += yDiff, frame.Size.Width, frame.Size.Height);
			view.Frame = newFrame;
			return view;
		}
		
		public static T MoveView<T>(this T view, PointF point, PointF otherPoint, PointF margin) where T : UIView
		{
			float xDiff = otherPoint.X - point.X;
			float yDiff = otherPoint.Y - point.Y;
			var frame = view.Frame;
			var newFrame = new RectangleF((frame.X += xDiff) + margin.X, (frame.Y += yDiff) + margin.Y, frame.Size.Width, frame.Size.Height);
			view.Frame = newFrame;
			return view;
		}
		
		/// <summary>
		/// Returns the point within the view designated by the provided anchor point.
		/// The provided view must not be nil.
		/// </summary>
		/// <param name="view">The view whose point is being returned.</param>
		/// <param name="anchorPoint">The view's anchor point.</param>
		/// <returns></returns>
		public static PointF PointForAnchor(this UIView view, AnchorPoint anchorPoint)
		{
			var frame = view.Frame;
			float x = 0.0f;
			float y = 0.0f;
			switch (anchorPoint)
			{
			case AnchorPoint.Left:
				x = frame.X;
				y = (frame.Size.Height / 2.0f) + frame.Y;
				break;
			case AnchorPoint.Center:
				x = (frame.Size.Width / 2.0f) + frame.X;
				y = (frame.Size.Height / 2.0f) + frame.Y;
				break;
			case AnchorPoint.TopRight:
				x = frame.Size.Width + frame.X;
				y = frame.Y;
				break;
			case AnchorPoint.TopLeft:
				x = frame.X;
				y = frame.Y;
				break;
			case AnchorPoint.Top:
				x = (frame.Size.Width / 2.0f) + frame.X;
				y = frame.Y;
				break;
			case AnchorPoint.Bottom:
				x = (frame.Size.Width / 2.0f) + frame.X;
				y = frame.Size.Height + frame.Y;
				break;
			case AnchorPoint.BottomRight:
				x = frame.Size.Width + frame.X;
				y = frame.Size.Height + frame.Y;
				break;
			case AnchorPoint.Right:
				x = frame.Size.Width + frame.X;
				y = (frame.Size.Height / 2.0f) + frame.Y;
				break;
			case AnchorPoint.BottomLeft:
				x = frame.X;
				y = frame.Size.Height + frame.Y;
				break;
			}
			return new PointF(x, y);
		}
		
		/// <summary>
		/// Find the first responder in the &lt;paramref name=&quot;view&quot;/&gt;'s subview hierarchy
		/// </summary>
		/// <param name="view"></param>
		/// <returns></returns>
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}
		
		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view"></param>
		/// <param name="stopAt"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsInstanceOfType(view.Superview))
				{
					return view.Superview;
				}
				
				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}
			
			return null;
		}
	}
}

