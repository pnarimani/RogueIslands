using System;
using System.Collections.Generic;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Evaluators;
using RogueIslands.Gameplay.Boosters.Executors;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.Rollback;

namespace RogueIslands.Gameplay.Modules
{
    public class GameplayCoreModule : IModule
    {
        public void Load(IContainerBuilder builder)
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
                .OnActivated((container, instance) => instance.SetExecutors(container.Resolve<IReadOnlyList<GameActionExecutor>>()));
            RegisterController<GameConditionsController>(builder)
                .OnActivated((container, instance) => instance.SetEvaluators(container.Resolve<IReadOnlyList<GameConditionEvaluator>>()));
            RegisterController<BoosterManagement>(builder);
            RegisterController<ResetController>(builder);
            RegisterController<WorldBoosterGeneration>(builder);
            RegisterController<BuildingPlacement>(builder);
            RegisterController<RoundController>(builder);
        }

        private static IRegistration<T> RegisterController<T>(IContainerBuilder builder)
        {
            return builder.RegisterType<T>()
                .SingleInstance()
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}