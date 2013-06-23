using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using IcucApp.Core;
using MonoTouch.Foundation;

namespace IcucApp
{
    public class LastUpdateElement : StyledStringElement, IElementSizing
    {
        public LastUpdateElement(DateTime timeStamp) : base("")
        {
            this.Caption = "Laatste update: {0:dd/MMM/yy H:mm}".FormatWith(timeStamp);
            this.Font = UIFont.FromName("HelveticaNeue", 11);
            this.TextColor = UIColor.DarkGray;
            this.Alignment = UITextAlignment.Center;
            this.BackgroundColor = UIColor.FromRGB(0xF4, 0xF4, 0xF4);
        }

        public float GetHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return 30;
        }
    }
}

