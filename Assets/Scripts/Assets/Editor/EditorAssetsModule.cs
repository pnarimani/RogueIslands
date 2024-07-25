using Autofac;
using RogueIslands.DependencyInjection;

namespace RogueIslands.Assets.Editor
{
    public class EditorAssetsModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourcesAssetSaver>().SingleInstance().AsImplementedInterfaces();
        }
    }
}
