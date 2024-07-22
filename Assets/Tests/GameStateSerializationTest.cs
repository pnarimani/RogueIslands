using System;
using System.Collections.Generic;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using RogueIslands.Gameplay;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.Rand;
using RogueIslands.Serialization;

namespace RogueIslands.Tests
{
    public class GameStateSerializationTest
    {
        private GameState _before;
        private ISerializer _serializer;
        private IDeserializer _deserializer;

        [SetUp]
        public void Setup()
        {
            var defaultBuildings = DefaultBuildingsList.Get();
            var inGameBuildings = DefaultBuildingsList.Get();
            foreach (var b in inGameBuildings)
            {
                b.Id = BuildingId.NewBuildingId();
            }

            var defaultBoosters = BoosterList.Get();
            var inGameBoosters = BoosterList.Get();
            foreach (var b in inGameBoosters)
            {
                b.Id = BoosterInstanceId.New();
            }
            
            _before = new GameState
            {
                Round = 5,
                Act = 67,
                HandSize = 3,
                CurrentScore = 45,
                AllRequiredScores = new double[]
                {
                    1, 2, 3, 4, 56, 6, 67,
                },
                Money = 45,
                MoneyPayoutPerRound = 54,
                MoneyChanges = new List<MoneyChange>()
                {
                    new MoneyChange
                    {
                        Change = 3,
                        Reason = "A"
                    }
                },
                CurrentEvent = new BoosterAdded()
                {
                    Booster = new BoosterCard
                    {
                        Id = new BoosterInstanceId(2),
                        Name = "BCDC",
                    },
                },
                Buildings = new BuildingsState
                {
                    ShufflingRandom = new RogueRandom(3245),
                    Deck = inGameBuildings ,
                    HandPointer = 0,
                    All = defaultBuildings,
                },
                MaxBoosters = 34234,
                Boosters = inGameBoosters,
                AvailableBoosters = defaultBoosters,
                Shop = new ShopState
                {
                    StartingRerollCost = 324,
                    CurrentRerollCost = 23,
                    CardCount = 3434,
                    ItemsForSale = new IPurchasableItem[]
                    {
                        new Consumable() { Name = "Consumable" },
                        new BoosterCard() { Name = "Booseter" },
                    },
                    SelectionRandom = new RogueRandom(523),
                    BoosterSpawn = new RogueRandom(232),
                    DeduplicationRandom = new RogueRandom(423),
                    CardPackSpawn = new RogueRandom(234),
                },
                Result = GameResult.InProgress,
                WorldBoosters = new WorldBoostersState
                {
                    SpawnRandom = new RogueRandom(54),
                    SelectionRandom = new RogueRandom(54),
                    PositionRandom = new RogueRandom(54),
                    SpawnDistribution = new PowerDistribution()
                    {
                        Factor = 0.98,
                        Power = 4.37,
                    },
                    SpawnCount = 10,
                    SpawnedBoosters = new List<WorldBooster>()
                    {
                        new WorldBooster()
                        {
                            Name = "WorldBooster",
                        }
                    },
                    All = WorldBoosterList.Get(),

                },
            };

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies());
            var container = builder.Build();
            _serializer = container.Resolve<ISerializer>();
            _deserializer = container.Resolve<IDeserializer>();
        }

        [Test]
        [Timeout(1000)]
        public void GameState_SerializationIntegrationTest()
        {
            var text = _serializer.Serialize(_before);

            var after = _deserializer.Deserialize<GameState>(text);

            after.Should().BeEquivalentTo(_before);
        }
    }
}