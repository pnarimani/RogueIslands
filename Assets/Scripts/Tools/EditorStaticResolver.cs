using Autofac;
using RogueIslands.DependencyInjection;
using RogueIslands.DependencyInjection.Autofac;

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
                foreach (var projectModule in ModuleFinder.GetProjectModules())
                {
                    projectModule.Load(containerBuilderProxy);
                }

                _container = builder.Build();
            }

            return _container.Resolve<T>();
        }
    }
}