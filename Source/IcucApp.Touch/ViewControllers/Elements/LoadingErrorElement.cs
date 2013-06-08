using System;
using MonoTouch.Dialog;

namespace IcucApp
{
    public class LoadingErrorElement : LoadMoreElement
    {
        public LoadingErrorElement(string title, string subTitle, string loadingCaption, Action<LoadMoreElement> action) : 
            base(title, loadingCaption, action)
        {
        }
    }
}

