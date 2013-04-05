using IcucApp.Core.Ioc;
using IcucApp.Core.Presentation;
using MonoTouch.UIKit;

namespace IcucApp.Core.Touch.UIKit
{
    public class MvpViewController<TPresenter> : UIViewController where TPresenter : class, IPresenter
    {
        private readonly TPresenter _presenter;

        public MvpViewController()
        {
            _presenter = Container.Resolve<TPresenter>(new NamedParameter() { { "view", this } });
        }

        public TPresenter Presenter
        {
            get { return _presenter; }
        }
    }
}