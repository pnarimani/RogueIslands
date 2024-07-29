using Autofac;
using RogueIslands.Autofac;

namespace RogueIslands.UISystem
{
    public class UISystemModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowOpener>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<WindowRegistry>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<UIRootProvider>().AsImplementedInterfaces().SingleInstance();
        }
    }
}