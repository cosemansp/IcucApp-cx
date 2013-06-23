using MonoTouch.Dialog;
using MonoTouch.Foundation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.UIKit;
using System;
using System.Drawing;

namespace IcucApp.ViewControllers.Elements
{
	public class WebViewElement : Element, IElementSizing
	{
		private static readonly NSString CellId = new NSString("WebViewElement");
		private UITableViewCell _cell;
		private UIWebView _webView;
		
		public WebViewElement(UIWebView webView): base(string.Empty)
		{
			_webView = webView;
			_webView.ScrollView.ScrollEnabled = false;
			_webView.Frame = new RectangleF(0, 5, _webView.Frame.Width, _webView.Frame.Height);
			_webView.SizeToFit();
            _webView.BackgroundColor = UIColor.Red;
			_webView.Alpha = 1.0f;
		}
		
		public override UITableViewCell GetCell (MonoTouch.UIKit.UITableView tv)
		{
			_cell = tv.DequeueReusableCell(CellId) as UITableViewCell;
			if (_cell == null) {
				_cell = new UITableViewCell(UITableViewCellStyle.Default, CellId); 
				_cell.ContentView.AddSubview(_webView);
				_cell.BackgroundColor = UIColor.White;
			}
			return _cell;
		}

		public float GetHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return _webView.Frame.Height + 15;
		}
	}
}