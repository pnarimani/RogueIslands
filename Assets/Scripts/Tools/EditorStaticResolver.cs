using Autofac;
using RogueIslands.Assets;
using RogueIslands.Assets.Editor;
using RogueIslands.DependencyInjection;
using RogueIslands.DependencyInjection.Autofac;
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
                var containerBuilderProxy = new ContainerBuilderProxy(builder);
                new AssetsModule().Load(containerBuilderProxy);
                new EditorAssetsModule().Load(containerBuilderProxy);
                new YamlSerializationModule().Load(containerBuilderProxy);
                _container = builder.Build();
            }

            return _container.Resolve<T>();
        }
    }
}