using System.Collections.Generic;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.View.DeckBuilding;
using UnityEngine;

namespace RogueIslands.Gameplay.View
{
    public class GameplayViewModule : IModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterMonoBehaviour<InputHandling>()
                .AutoActivate()
                .SingleInstance();
            
            builder.RegisterMonoBehaviour<CardIdleAnimationManager>()
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

            builder.RegisterMonoBehaviour<GizmosCaller>()
                .AutoActivate()
                .OnActivated((container, instance) => instance.Initialize(container.Resolve<IReadOnlyList<IGizmosDrawer>>()));

            builder.RegisterType<GameManager>()
                .AutoActivate()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<DeckBuildingView>()
                .AutoActivate()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<PlayButtonHandler>().AsImplementedInterfaces().AsSelf().SingleInstance().AutoActivate();
        }
    }
}