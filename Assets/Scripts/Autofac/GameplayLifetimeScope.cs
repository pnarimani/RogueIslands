using Autofac;
using AutofacUnity;
using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands.Autofac
{
    public class GameplayLifetimeScope : AutofacScope, IResolver
    {
        [SerializeField] private string _seed;

        public string Seed
        {
            get => _seed;
            set => _seed = value;
        }

        protected override void Configure(ContainerBuilder builder)
        {
            var seedRandom = new System.Random(Seed.GetHashCode());
            builder.RegisterInstance(seedRandom).As<System.Random>();
            
            builder.RegisterModule<BoostersModule>();
        }

        public T Resolve<T>() => Container.Resolve<T>();
    }
}