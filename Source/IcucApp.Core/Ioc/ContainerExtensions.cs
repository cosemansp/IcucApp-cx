
namespace IcucApp.Core.Ioc
{
    public static class ContainerExtensions
    {
         public static void RegisterModule(this TinyIoCContainer container, IRegistrationModule module)
         {
             module.Initialize(container);
         }
    }

    public interface IRegistrationModule
    {
        void Initialize(TinyIoCContainer container);
    }
}