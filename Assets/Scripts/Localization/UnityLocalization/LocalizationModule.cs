using RogueIslands.DependencyInjection;

namespace RogueIslands.Localization.UnityLocalization
{
    public class LocalizationModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<UnityLocalizationAdapter>().AsImplementedInterfaces().SingleInstance();
        }
    }
}