#if UNITY_EDITOR
using RogueIslands.DependencyInjection;

namespace RogueIslands.Assets.Editor
{
    public class EditorAssetsModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ResourcesAssetSaver>().SingleInstance().AsImplementedInterfaces();
        }
    }
}
#endif