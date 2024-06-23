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
        [SerializeField] private bool _useRandomSeed;
        [SerializeField] private string _seed;
        [SerializeField] private GameManager _gameManagerPrefab;

        public string Seed => _seed;

        protected override void Configure(ContainerBuilder builder)
        {
            builder.RegisterInstance(_useRandomSeed ? new Random() : new Random(Seed.GetHashCode()));

            builder.RegisterModule<GameplayCoreModule>();

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
                .OnActivated(m =>
                {
                    m.Instance.Initialize(
                        m.Context.Resolve<GameState>(),
                        m.Context.Resolve<PlayController>(),
                        m.Context.Resolve<BoosterManagement>()
                    );
                })
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        public T Resolve<T>() => Container.Resolve<T>();
    }
}