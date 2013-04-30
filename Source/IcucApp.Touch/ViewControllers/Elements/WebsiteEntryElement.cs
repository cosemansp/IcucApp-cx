using MonoTouch.Dialog;
using MonoTouch.Foundation;

namespace IcucApp.ViewControllers.Elements
{
    public class WebsiteEntryElement : StyledStringElement
    {
        public WebsiteEntryElement(string caption)
            : base(caption)
        {
        }

        public WebsiteEntryElement(string caption, NSAction tapped)
            : base(caption, tapped)
        {
        }
    }
}