using Autofac;
using RogueIslands.Assets;
using RogueIslands.Assets.Editor;
using RogueIslands.Serialization.YamlDotNetIntegration;

namespace RogueIslands.Tools
{
    public static class EditorStaticResolver
    {
        private static IComponentContext _container;

        public static T Resolve<T>()
        {
            if (_container == null)
            {
                var builder = new ContainerBuilder();
                new AssetsModule().Load(builder);
                new EditorAssetsModule().Load(builder);
                new YamlSerializationModule().Load(builder);
                _container = builder.Build();
            }

            return _container.Resolve<T>();
        }
    }
}