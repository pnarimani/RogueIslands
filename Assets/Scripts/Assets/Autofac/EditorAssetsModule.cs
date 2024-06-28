#if UNITY_EDITOR
using Autofac;
using RogueIslands.Assets.Editor;

namespace Assets.Autofac
{
    public class EditorAssetsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourcesAssetSaver>().AsImplementedInterfaces();
        }
    }
}
#endif