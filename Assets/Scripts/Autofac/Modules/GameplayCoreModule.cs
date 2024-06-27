using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Executors;
using RogueIslands.Buildings;
using RogueIslands.DeckBuilding;
using RogueIslands.Rollback;
using RogueIslands.Serialization.DeepClone;

namespace RogueIslands.Autofac.Modules
{
    public class GameplayCoreModule : Module
    {
        private readonly Seed _seed;

        public GameplayCoreModule(Seed seed) => _seed = seed;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new Cloner()).AsImplementedInterfaces().SingleInstance();
            
            builder.RegisterInstance(_seed);
            
            builder.Register(c => new Random(_seed.Value.GetHashCode()))
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