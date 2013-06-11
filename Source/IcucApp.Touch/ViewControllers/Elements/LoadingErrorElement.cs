using System;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace IcucApp
{
    public class LoadingErrorElement : Element, IElementSizing
    {
        private static readonly NSString CellId = new NSString("LoadingErrorElement");
        private string _title;
        private string _subTitle;
        public event EventHandler Tapped;
        private LoadingErrorCell _cell;


        public LoadingErrorElement(string title, string subTitle) : base(title) 
        {
            _title = title;
            _subTitle = subTitle;
        }   

        public LoadingErrorElement() : base("") 
        {
            _title = "Oops, we kregen een fout tijdens het laden van de informatie. Mogelijk is er geen internet verbinding.";
            _subTitle = "Tap om opnieuw te proberen.";
        }


        public override MonoTouch.UIKit.UITableViewCell GetCell (MonoTouch.UIKit.UITableView tv)
        {
            _cell = tv.DequeueReusableCell(CellId) as LoadingErrorCell;
            if (_cell == null) 
                _cell = new LoadingErrorCell(UITableViewCellStyle.Default, CellId, _title, _subTitle); 
            else 
                _cell.Update(_title, _subTitle);
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
            return 100;
        }
    }
}

