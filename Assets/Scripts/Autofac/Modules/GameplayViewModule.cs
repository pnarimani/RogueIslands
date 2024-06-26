using System.Collections.Generic;
using System.Diagnostics;
using Autofac;
using AutofacUnity;
using RogueIslands.View;
using RogueIslands.View.DeckBuilding;
using UnityEngine;

namespace RogueIslands.Autofac.Modules
{
    public class GameplayViewModule : Module
    {
        private readonly GameManager _gameManagerPrefab;
        private readonly DeckBuildingView _deckBuildingView;

        public GameplayViewModule(GameManager gameManagerPrefab, DeckBuildingView deckBuildingView)
        {
            _deckBuildingView = deckBuildingView;
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
            
            builder.Register(_ => Object.Instantiate(_deckBuildingView))
                .AutoActivate()
                .OnActivated(m =>
                {
                })
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}