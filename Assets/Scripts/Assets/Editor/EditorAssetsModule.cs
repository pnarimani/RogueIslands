using Autofac;
using RogueIslands.Autofac;

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
