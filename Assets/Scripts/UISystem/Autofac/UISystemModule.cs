using Autofac;
using RogueIslands.UISystem;

namespace UISystem.Autofac
{
    public class UISystemModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowOpener>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<WindowRegistry>().AsImplementedInterfaces().SingleInstance();
        }
    }
}