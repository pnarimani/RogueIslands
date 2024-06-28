using System;
using Autofac;
using AutofacUnity;
using RogueIslands.Gameplay;
using UnityEngine;

namespace RogueIslands.Autofac
{
    public class GameplayLifetimeScope : AutofacScope, IResolver
    {
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seed;

        protected override void Configure(ContainerBuilder builder)
        {
            var seed = new Seed(_useRandomSeed ? Environment.TickCount.ToString() : _seed);
            builder.RegisterInstance(seed);

            foreach (var instance in ModuleFinder.GetGameplayModules())
            {
                builder.RegisterModule(instance);
            }
        }

        public T Resolve<T>() => Container.Resolve<T>();
    }
}