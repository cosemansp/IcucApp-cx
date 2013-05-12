using MonoTouch.Dialog;
using MonoTouch.Foundation;
using IcucApp.Presentation.ViewModels;
using System;
using MonoTouch.UIKit;

namespace IcucApp.ViewControllers.Elements
{
	public class FacebookEntryElement : Element, IElementSizing
	{
		private static readonly NSString CellId = new NSString("FacebookEntryElement");
		private FeedData _data;
		private FacebookEntryCell _cell;
		public event EventHandler Tapped;
		
		public FeedData Data {
			get {
				return _data;
			}
		}
		
		public FacebookEntryElement(FeedData data): base(data.Title)
			
		{
			_data = data;
		}
		
		public override MonoTouch.UIKit.UITableViewCell GetCell (MonoTouch.UIKit.UITableView tv)
		{
			_cell = tv.DequeueReusableCell(CellId) as FacebookEntryCell;
			if (_cell == null) 
				_cell = new FacebookEntryCell(UITableViewCellStyle.Default, CellId, _data); 
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
			return FacebookEntryCell.Margin + WebsiteEntryCell.ImageHeigth + WebsiteEntryCell.Margin;
		}
	}
}