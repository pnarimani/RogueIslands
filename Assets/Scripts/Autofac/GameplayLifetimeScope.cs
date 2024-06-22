using System.Collections.Generic;
using Autofac;
using AutofacUnity;
using RogueIslands.View;
using UnityEngine;
using Random = System.Random;

namespace RogueIslands.Autofac
{
    public class GameplayLifetimeScope : AutofacScope, IResolver
    {
        [SerializeField] private string _seed;
        [SerializeField] private GameManager _gameManagerPrefab;
        
        public string Seed => _seed;

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Random(Seed.GetHashCode()));
            builder.Register((c) => c.Resolve<Random>().NextRandom())
                .InstancePerDependency();
            
            builder.RegisterModule<BoostersModule>();
            builder.RegisterModule<RollbackModule>();

            builder.Register(_ => GameFactory.NewGame(new Random(Seed.GetHashCode())))
                .SingleInstance()
                .AsSelf();
            
            builder.RegisterMonoBehaviour<InputHandling>()
                .AutoActivate()
                .SingleInstance();

            builder.Register(_ => new AnimationScheduler())
                .AutoActivate()
                .SingleInstance();
            
            
            builder.Register(_ => new BuildingViewPlacement())
                .AutoActivate()
                .SingleInstance()
                .AsImplementedInterfaces()
                .AsSelf();
            
            builder.Register(c => new GameObject().AddComponent<GizmosCaller>())
                .AutoActivate()
                .OnActivated(c => c.Instance.Initialize(c.Context.Resolve<IReadOnlyList<IGizmosDrawer>>()));

            builder.Register(_ => Instantiate(_gameManagerPrefab))
                .AutoActivate()
                .OnActivating(m => m.Instance.SetState(m.Context.Resolve<GameState>()))
                .AsSelf()
                .AsImplementedInterfaces();
        }

        public T Resolve<T>() => Container.Resolve<T>();
    }
}