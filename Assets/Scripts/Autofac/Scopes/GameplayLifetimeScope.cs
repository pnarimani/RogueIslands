using Autofac;
using RogueIslands.Gameplay;
using UnityEngine;

namespace RogueIslands.Autofac.Scopes
{
    public class GameplayLifetimeScope : StaticallyResolvableLifetimeScope
    {
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seed;

        protected override void Configure(ContainerBuilder builder)
        {
            if (!_useRandomSeed)
                builder.RegisterInstance(new Seed(_seed));       
            
            foreach (var instance in ModuleFinder.GetGameplayModules())
            {
                instance.Load(builder);
            }
        }
    }
}