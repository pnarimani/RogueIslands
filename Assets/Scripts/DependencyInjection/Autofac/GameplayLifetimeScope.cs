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
            if (!_useRandomSeed)
                builder.RegisterInstance(new Seed(_seed));
            
            var builderProxy = new ContainerBuilderProxy(builder);
            foreach (var instance in ModuleFinder.GetGameplayModules())
            {
                instance.Load(builderProxy);
            }
        }

        public T Resolve<T>()
        {
            if (Container == null)
                Build();
            return Container!.Resolve<T>();
        }
    }
}