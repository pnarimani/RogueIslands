using System;
using Autofac;
using AutofacUnity;
using RogueIslands.Autofac.Modules;
using RogueIslands.View;
using UnityEngine;

namespace RogueIslands.Autofac
{
    public class GameplayLifetimeScope : AutofacScope, IResolver
    {
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seed;
        [SerializeField] private GameManager _gameManagerPrefab;


        protected override void Configure(ContainerBuilder builder)
        {
            var seed = new Seed(_useRandomSeed ? Environment.TickCount.ToString() : _seed);
            builder.RegisterModule(new GameplayCoreModule(seed));
            builder.RegisterModule(new GameplayViewModule(_gameManagerPrefab));
            builder.RegisterModule<BoostersModule>();
            builder.RegisterModule<RollbackModule>();
            builder.RegisterModule<SerializationModule>();
        }

        public T Resolve<T>() => Container.Resolve<T>();
    }
}