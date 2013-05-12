using MonoTouch.Dialog;
using MonoTouch.Foundation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.UIKit;
using System;

namespace IcucApp.ViewControllers.Elements
{
	public class WebsiteEntryElement : Element, IElementSizing
	{
		private static readonly NSString CellId = new NSString("WebsiteEntryElement");
		private FeedData _data;
		private WebsiteEntryCell _cell;
		public event EventHandler Tapped;
		
		public FeedData Data {
			get {
				return _data;
			}
		}
		
		public WebsiteEntryElement(FeedData data): base(data.Title)
			
		{
			_data = data;
		}
		
		public override MonoTouch.UIKit.UITableViewCell GetCell (MonoTouch.UIKit.UITableView tv)
		{
			_cell = tv.DequeueReusableCell(CellId) as WebsiteEntryCell;
			if (_cell == null) 
				_cell = new WebsiteEntryCell(UITableViewCellStyle.Default, CellId, _data); 
			else 
				_cell.Update(_data);
			return _cell;
		}

		public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			if (Tapped != null)
				Tapped(this, EventArgs.Empty);
			
			tableView.DeselectRow(indexPath, true);
		}

		public float GetHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return WebsiteEntryCell.Margin + WebsiteEntryCell.ImageHeigth + WebsiteEntryCell.Margin + 20;
		}
	}
}