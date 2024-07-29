using Autofac;
using RogueIslands.Autofac;

namespace RogueIslands.Localization.UnityLocalization
{
    public class LocalizationModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnityLocalizationAdapter>().AsImplementedInterfaces().SingleInstance();
        }
    }
}