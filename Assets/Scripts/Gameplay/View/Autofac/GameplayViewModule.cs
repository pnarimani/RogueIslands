using System.Collections.Generic;
using Autofac;
using AutofacUnity;
using RogueIslands.Gameplay.View.DeckBuilding;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Autofac
{
    public class GameplayViewModule : Module
    {
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