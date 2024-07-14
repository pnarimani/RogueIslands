using Autofac;
using RogueIslands.Gameplay;
using UnityEngine;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class GameplayLifetimeScope : StaticallyResolvableLifetimeScope
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