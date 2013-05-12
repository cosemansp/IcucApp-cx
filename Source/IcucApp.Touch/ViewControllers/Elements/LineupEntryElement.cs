using MonoTouch.Dialog;
using MonoTouch.Foundation;
using IcucApp.Presentation.ViewModels;
using MonoTouch.UIKit;

namespace IcucApp.ViewControllers.Elements
{
    public class LineupEntryElement : Element, IElementSizing
    {
		private static readonly NSString CellId = new NSString("LineupEntryElement");
		private FeedData _data;
		private LineupEntryCell _cell;

		public FeedData Data {
			get {
				return _data;
			}
		}

		public LineupEntryElement(FeedData data): base(data.Title)
			 
        {
			_data = data;
        }

		public override MonoTouch.UIKit.UITableViewCell GetCell (MonoTouch.UIKit.UITableView tv)
		{
			_cell = tv.DequeueReusableCell(CellId) as LineupEntryCell;
			if (_cell == null) 
				_cell = new LineupEntryCell(UITableViewCellStyle.Default, CellId, _data); 
			else 
				_cell.Update(_data);
			return _cell;
		}

		public float GetHeight(UITableView tableView, NSIndexPath indexPath)
		{
			return LineupEntryCell.Margin + LineupEntryCell.ImageHeigth + LineupEntryCell.Margin;
		}
    }
}