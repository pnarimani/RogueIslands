using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Evaluators;
using RogueIslands.Gameplay.Boosters.Executors;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.Rollback;

namespace RogueIslands.Gameplay.Autofac
{
    public class GameplayCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Random())
                .SingleInstance();
            
            builder.Register((c) => c.Resolve<Random>().NextRandom())
                .InstancePerDependency();
            
            builder.Register(c => GameFactory.NewGame(c.Resolve<Random>()))
                            .SingleInstance()
                            .AsSelf();

            RegisterController<PlayController>(builder);
            RegisterController<EventController>(builder).AsImplementedInterfaces();
            RegisterController<GameActionController>(builder)
                .OnActivated(c => c.Instance.SetExecutors(c.Context.Resolve<IReadOnlyList<GameActionExecutor>>()));
            RegisterController<GameConditionsController>(builder)
                .OnActivated(c => c.Instance.SetEvaluators(c.Context.Resolve<IReadOnlyList<GameConditionEvaluator>>()));
            RegisterController<BoosterManagement>(builder);
            RegisterController<ResetController>(builder);
            RegisterController<WorldBoosterGeneration>(builder);
            RegisterController<BuildingPlacement>(builder);
            RegisterController<DeckBuildingController>(builder);
            RegisterController<RoundController>(builder);
        }

        private static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterController<T>(ContainerBuilder builder)
        {
            return builder.RegisterType<T>()
                .SingleInstance()
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}