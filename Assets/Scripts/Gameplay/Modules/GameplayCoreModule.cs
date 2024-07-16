﻿using System;
using System.Collections.Generic;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Evaluators;
using RogueIslands.Gameplay.Boosters.Executors;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Rand;
using RogueIslands.Gameplay.Rollback;
using RogueIslands.Gameplay.Shop;
using UnityEngine;

namespace RogueIslands.Gameplay.Modules
{
    public class GameplayCoreModule : IModule
    {
        public void Load(IContainerBuilder builder)
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

            RegisterController<PlayController>(builder);
            RegisterController<EventController>(builder).AsImplementedInterfaces();
            RegisterController<GameActionController>(builder)
                .OnActivated((container, instance) =>
                    instance.SetExecutors(container.Resolve<IReadOnlyList<GameActionExecutor>>()));
            RegisterController<GameConditionsController>(builder)
                .OnActivated((container, instance) =>
                    instance.SetEvaluators(container.Resolve<IReadOnlyList<GameConditionEvaluator>>()));
            RegisterController<BoosterManagement>(builder);
            RegisterController<ResetController>(builder);
            RegisterController<WorldBoosterGeneration>(builder);
            RegisterController<BuildingPlacement>(builder);
            RegisterController<RoundController>(builder);
            RegisterController<ShopRerollController>(builder);
            RegisterController<ShopPurchaseController>(builder);
            RegisterController<ShopItemSpawner>(builder);
            RegisterController<DiscardController>(builder);
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