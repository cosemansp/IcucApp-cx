namespace IcucApp.Core.Presentation
{
    public interface INavigator
    {
        void PushPresenter<T>(IView view, object state);
        void OpenPresenter<T>(IView view, object state);
    }
}