using System;
using Autofac;

namespace RogueIslands.Tools
{
    public static class EditorStaticResolver
    {
        public static T Resolve<T>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies());
            var container = builder.Build();
            return container.Resolve<T>();
        }
    }
}