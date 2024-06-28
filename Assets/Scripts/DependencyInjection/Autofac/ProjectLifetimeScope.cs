using Autofac;
using AutofacUnity;
using IngameDebugConsole;
using UnityEngine;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class ProjectLifetimeScope : AutofacScope
    {
        [SerializeField] private DebugLogManager _debugConsole;

        protected override void Configure(ContainerBuilder builder)
        {
            var builderProxy = new ContainerBuilderProxy(builder);
            foreach (var instance in ModuleFinder.GetProjectModules())
            {
                instance.Load(builderProxy);
            }

            builder.Register(_ => Instantiate(_debugConsole))
                .AutoActivate()
                .SingleInstance();
        }
    }
}