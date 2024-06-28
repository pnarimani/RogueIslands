using Autofac;
using RogueIslands.Autofac;

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
                foreach (var projectModule in ModuleFinder.GetProjectModules())
                {
                    builder.RegisterModule(projectModule);
                }

                _container = builder.Build();
            }

            return _container.Resolve<T>();
        }
    }
}