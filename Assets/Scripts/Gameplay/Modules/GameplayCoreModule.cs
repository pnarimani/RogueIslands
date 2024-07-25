using System.Collections.Generic;
using Autofac;
using Autofac.Builder;
using RogueIslands.DependencyInjection;
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

            builder.Register(c =>
                {
                    var seed = (uint)c.Resolve<Seed>().GetHashCode();
                    var seedRandom = new RogueRandom(seed);
                    return GameFactory.NewGame(seedRandom);
                })
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

            // const string dryRunName = "DryRun";
            // builder.RegisterType<GameState>().Named(dryRunName);

            // builder.Register(c =>
            //     {
            //         var fakeGame = new GameState();
            //         var realGame = c.Resolve<GameState>();
            //         var fakeView = new PreviewGameView(c.Resolve<IGameView>());
            //         var conditionsController = new GameConditionsController(fakeGame);
            //         var gameActionController = new GameActionController(fakeGame, fakeView, conditionsController);
            //         var eventController = new EventController(fakeGame, gameActionController);
            //         var scoring = c.Resolve<ScoringController>(fakeGame, fakeView, eventController);
            //         var cloner = c.Resolve<ICloner>();
            //         return new PreviewScoringController(fakeGame, realGame, scoring, cloner, fakeView);
            //     })
            //     .SingleInstance()
            //     .AsSelf();
        }

        private static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterController<T>(ContainerBuilder builder) =>
            builder.RegisterType<T>()
                .InstancePerLifetimeScope()
                .AsSelf()
                .AsImplementedInterfaces();
    }
}