namespace IcucApp.Core.UI
{
    public interface INavigator
    {
        void PushPresenter<T>(IView view, object state);
        void OpenPresenter<T>(IView view, object state);
    }
}