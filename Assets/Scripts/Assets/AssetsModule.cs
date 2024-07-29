using Autofac;
using RogueIslands.Autofac;

namespace RogueIslands.Assets
{
    public class AssetsModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourcesAssetLoader>().SingleInstance().AsImplementedInterfaces();
        }
    }
}