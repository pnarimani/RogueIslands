using Autofac;
using AutofacUnity;
using RogueIslands.Gameplay;
using UnityEngine;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class GameplayLifetimeScope : AutofacScope, IResolver
    {
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seed;

        protected override void Configure(ContainerBuilder builder)
        {
            var builderProxy = new ContainerBuilderProxy(builder);
            foreach (var instance in ModuleFinder.GetGameplayModules())
            {
                instance.Load(builderProxy);
            }
        }

        public T Resolve<T>() => Container.Resolve<T>();
    }
}