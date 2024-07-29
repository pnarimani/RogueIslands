using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Evaluators;
using RogueIslands.Gameplay.Boosters.Executors;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.DryRun;
using RogueIslands.Gameplay.Rand;
using RogueIslands.Gameplay.Shop;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.Modules
{
    public class GameplayCoreModule : IModule
    {
        public void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(Seed.GenerateRandom())
                .IfNotRegistered(typeof(Seed));

            builder.Register(c => GameFactory.NewGame(c.Resolve<Seed>()))
                .SingleInstance()
                .AsSelf();

            RegisterController<ScoringController>(builder);
            RegisterController<EventController>(builder);
            RegisterController<GameActionController>(builder);
            RegisterController<GameConditionsController>(builder);
            RegisterController<BoosterManagement>(builder);
            RegisterController<BuildingPlacement>(builder);
            RegisterController<RoundController>(builder);
            RegisterController<ShopRerollController>(builder);
            RegisterController<ShopPurchaseController>(builder);
            RegisterController<ShopItemSpawner>(builder);
            RegisterController<CardPackSpawner>(builder);
            RegisterController<DryRunScoringController>(builder);
            builder.RegisterType<DryRunGameView>().AsSelf().SingleInstance();
        }

        private static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterController<T>(ContainerBuilder builder) =>
            builder.RegisterType<T>()
                .InstancePerLifetimeScope()
                .AsSelf()
                .AsImplementedInterfaces();
    }
}