using Autofac;
using AutofacUnity;
using RogueIslands.Boosters;
using RogueIslands.View;
using UnityEngine;
using Random = System.Random;

namespace RogueIslands.Autofac
{
    public class GameplayLifetimeScope : AutofacScope, IResolver
    {
        [SerializeField] private string _seed;
        [SerializeField] private GameManager _gameManagerPrefab;
        
        public string Seed
        {
            get => _seed;
            set => _seed = value;
        }

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Random(Seed.GetHashCode()));
            builder.Register((c) => c.Resolve<Random>().NextRandom())
                .InstancePerDependency();
            
            builder.RegisterModule<BoostersModule>();

            builder.Register(_ => GameFactory.NewGame(new Random(Seed.GetHashCode())))
                .SingleInstance()
                .AsSelf();
            
            builder.RegisterMonoBehaviour<InputHandling>()
                .AutoActivate()
                .SingleInstance();

            builder.Register(_ => Instantiate(_gameManagerPrefab))
                .AutoActivate()
                .OnActivating(m => m.Instance.SetState(m.Context.Resolve<GameState>()))
                .AsSelf()
                .AsImplementedInterfaces();
        }

        public T Resolve<T>() => Container.Resolve<T>();
    }
}