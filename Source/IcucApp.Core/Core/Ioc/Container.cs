using System;
using System.Collections;
using System.Collections.Generic;

namespace IcucApp.Core.Ioc
{
    public interface IContainerContext
    {
        IEnumerable<TService> ResolveAll<TService>() where TService : class;
        IEnumerable ResolveAll(Type type);
        TService Resolve<TService>() where TService : class;
        TService Resolve<TService>(NamedParameter parameters) where TService : class;
        object TryResolve(Type type);
    }

    /// <summary>
    /// TinyIocContaine wrapper
    /// </summary>
    internal class ContainerContext : IContainerContext
    {
        private readonly TinyIoCContainer _container;

        public ContainerContext(TinyIoCContainer container)
        {
            _container = container;
        }

        public IEnumerable<TService> ResolveAll<TService>() where TService : class
        {
            return _container.ResolveAll<TService>();
        }

        public IEnumerable ResolveAll(Type type)
        {
            return _container.ResolveAll(type);
        }

        public TService Resolve<TService>() where TService : class
        {
            return _container.Resolve<TService>();
        }

        public TService Resolve<TService>(NamedParameter parameters) where TService : class
        {
            return _container.Resolve<TService>(parameters);
        }

        public object TryResolve(Type type)
        {
            object resolvedType;
            if (TinyIoCContainer.Current.TryResolve(type, out resolvedType))
                return resolvedType;
            return null;
        }
    }

    public static class Container
    {
        public static void Initialize(Action<TinyIoCContainer> registrationAction)
        {
            registrationAction.Invoke(TinyIoCContainer.Current);
            TinyIoCContainer.Current.Register<IContainerContext>(new ContainerContext(TinyIoCContainer.Current));
        }

        public static IEnumerable<TService> ResolveAll<TService>() where TService : class
        {
            return TinyIoCContainer.Current.ResolveAll<TService>();
        }

        public static IEnumerable ResolveAll(Type type)
        {
            return TinyIoCContainer.Current.ResolveAll(type);
        }

        public static TService Resolve<TService>() where TService : class
        {
            return TinyIoCContainer.Current.Resolve<TService>();
        }

        public static TService Resolve<TService>(NamedParameter parameters) where TService : class
        {
            return TinyIoCContainer.Current.Resolve<TService>(parameters);
        }

        public static TService Resolve<TService>(string name) where TService : class
        {
            return TinyIoCContainer.Current.Resolve<TService>(name);
        }

        public static object TryResolve(Type type)
        {
            object resolvedType;
            if (TinyIoCContainer.Current.TryResolve(type, out resolvedType))
                return resolvedType;
            return null;
        }
    }
}