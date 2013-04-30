using MonoTouch.Dialog;
using MonoTouch.Foundation;

namespace IcucApp.ViewControllers.Elements
{
    public class FacebookEntryElement : StyledStringElement
    {
        public FacebookEntryElement(string caption) : base(caption)
        {
        }

        public FacebookEntryElement(string caption, NSAction tapped) : base(caption, tapped)
        {
        }
    }
}