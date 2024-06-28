using RogueIslands.DependencyInjection;

namespace RogueIslands.Assets
{
    public class AssetsModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ResourcesAssetLoader>().SingleInstance().AsImplementedInterfaces();
        }
    }
}