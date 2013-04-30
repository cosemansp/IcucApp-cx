using System;
using System.Collections.Generic;
using System.Linq;
using IcucApp.Core.Diagnostics;
using MonoTouch.UIKit;

namespace IcucApp.Core.UI
{
    /// <summary>
    /// Defines the Presenter/ViewController routes
    /// <example>
    /// <![CDATA[
    /// Route.MapRoute<ArticlePresenter>("ArticlePage", state => new ArticleViewController(state), true /* animated */)
    /// Route.MapRoute<SectionPresenter>("ArticlePage", state => new SectionViewController(state), true /* animated */)
    /// ]]></example>
    /// </summary>
    public static class RouteConfig
    {
        internal static readonly List<Route> Routes = new List<Route>();

        internal class Route
        {
           public Type PresenterType { get; set; }
           public string Name { get; set; }
           public bool Animated { get; set; }
           public Func<string, UIViewController> Func { get; set; }
        }

        public static void MapRoute<TPresenter>(string name, Func<string, UIViewController> func, bool animated) where TPresenter : IPresenter
        {
            if (Routes.Any(route => route.PresenterType == typeof (TPresenter)))
                throw new InvalidRouteException("Route to {0} already exist.".FormatWith(typeof (TPresenter).Name));

            Routes.Add(new Route
            {
                PresenterType = typeof(TPresenter),
                Name = name,
                Func = func,
                Animated = animated
            });
        }
    }

    public class RouteException : Exception
    {
        public RouteException(string message)
            : base(message)
        {
        }
    }

    public class InvalidRouteException : RouteException
    {
        public InvalidRouteException(string message) : base(message)
        {
        }
    }

    public class UnknownRouteException : RouteException
    {
        public UnknownRouteException(string message)
            : base(message)
        {
        }
    }

    public class TouchNavigator : INavigator
    {
        private readonly UIApplication _app;
        private readonly ILog _log = LogManager.GetLogger(typeof (TouchNavigator));

        public TouchNavigator(UIApplication app)
        {
            _app = app;
        }

        public void OpenPresenter<T>(IView view, object state)
        {
            _log.InfoFormat("OpenPresenter: {0} with {1}", typeof(T).Name, state);

            // serialize to json
            var serializedData = state as string;
            if (serializedData == null && state != null)
            {
                serializedData = state.ToJson();
            }

            var navigationController = GetCurrentController(view);
            if (navigationController == null)
                throw new RouteException("Missing UINavigationController, unable to navigate");

            var route = RouteConfig.Routes.FirstOrDefault(x => x.PresenterType == typeof(T));
            if (route == null)
                throw new UnknownRouteException("Route for {0} is not found.".FormatWith(typeof(T).Name));

            var isAnimated = route.Animated;
            var controller = route.Func(serializedData);
            navigationController.PresentViewController(controller, isAnimated, null);
        }

        public void PushPresenter<T>(IView view, object state)
        {
            _log.InfoFormat("PushPresenter: {0} with {1}", typeof (T).Name, state);

            // serialize to json
            var serializedData = state as string;
            if (serializedData == null && state != null)
            {
                serializedData = state.ToJson();
            }

            var navigationController = GetCurrentController(view);
            if (navigationController == null)
                throw new RouteException("Missing UINavigationController, unable to navigate");

            var route = RouteConfig.Routes.FirstOrDefault(x => x.PresenterType == typeof (T));
            if (route == null)
                throw new UnknownRouteException("Route for {0} is not found.".FormatWith(typeof(T).Name));

            var isAnimated = route.Animated;
            var controller = route.Func(serializedData);
            navigationController.PushViewController(controller, isAnimated);
        }

        private UINavigationController GetCurrentController(IView view)
        {
            if (view != null)
            {
                // get the navigation controller from the view
                var viewController = view as UIViewController;
                if (viewController != null) 
                    return viewController.NavigationController;
            }

            // no view supplied, try to get it from the mainWindow
            var mainWindows = _app.KeyWindow;
            if (mainWindows != null)
            {
                var rootController = mainWindows.RootViewController;
                if (rootController is UINavigationController)
                    return rootController as UINavigationController;
                var controller = rootController as UITabBarController;
                if (controller != null)
                {
                    var selectedViewController = controller.SelectedViewController;
                    if (selectedViewController is UINavigationController)
                        return selectedViewController as UINavigationController;
                }
            }

            // not navigation controller
            return null;
        }

    }
}