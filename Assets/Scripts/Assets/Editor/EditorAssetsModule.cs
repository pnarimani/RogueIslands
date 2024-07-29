using Autofac;
using RogueIslands.Autofac;

namespace RogueIslands.Assets.Editor
{
    public class EditorAssetsModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddressableSaver>().SingleInstance().AsImplementedInterfaces();
        }
    }
}
