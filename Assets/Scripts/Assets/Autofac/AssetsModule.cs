using Autofac;
using RogueIslands.Assets;

namespace Assets.Autofac
{
    public class AssetsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourcesAssetLoader>().AsImplementedInterfaces();
        }
    }
}