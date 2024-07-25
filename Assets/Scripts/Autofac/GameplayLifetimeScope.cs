using Autofac;
using UnityEngine;

namespace RogueIslands.DependencyInjection.Autofac
{
    public class GameplayLifetimeScope : StaticallyResolvableLifetimeScope
    {
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seed;

        protected override void Configure(ContainerBuilder builder)
        {
            foreach (var instance in ModuleFinder.GetGameplayModules())
            {
                instance.Load(builder);
            }
        }
    }
}