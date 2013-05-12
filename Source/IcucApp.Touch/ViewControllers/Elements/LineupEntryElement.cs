using MonoTouch.Dialog;
using MonoTouch.Foundation;

namespace IcucApp.ViewControllers.Elements
{
    public class LineupEntryElement : StyledStringElement
    {
		public LineupEntryElement(string caption) : base(caption)
        {
        }

		public LineupEntryElement(string caption, NSAction tapped) : base(caption, tapped)
        {
        }
    }
}