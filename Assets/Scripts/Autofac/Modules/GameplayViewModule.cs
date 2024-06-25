using System.Collections.Generic;
using System.Diagnostics;
using Autofac;
using AutofacUnity;
using RogueIslands.View;
using UnityEngine;

namespace RogueIslands.Autofac.Modules
{
    public class GameplayViewModule : Module
    {
        private GameManager _gameManagerPrefab;

        public GameplayViewModule(GameManager gameManagerPrefab)
        {
            _gameManagerPrefab = gameManagerPrefab;
        }
        
        protected override void Load(ContainerBuilder builder)
        {
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

            builder.Register(_ => Object.Instantiate(_gameManagerPrefab))
                .AutoActivate()
                .OnActivated(m =>
                {
                    m.Instance.Initialize(
                        m.Context.Resolve<GameState>(),
                        m.Context.Resolve<PlayController>()
                    );
                })
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}