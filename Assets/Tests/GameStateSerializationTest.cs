using System.Collections.Generic;
using Autofac;
using FluentAssertions;
using NUnit.Framework;
using RogueIslands;
using RogueIslands.Autofac.Modules;
using RogueIslands.Boosters;
using RogueIslands.Buildings;
using RogueIslands.DeckBuilding;
using RogueIslands.GameEvents;
using RogueIslands.Serialization;
using Random = Unity.Mathematics.Random;

namespace Tests
{
    public class GameStateSerializationTest
    {
        private GameState _before;
        private ISerializer _serializer;
        private IDeserializer _deserializer;

        [SetUp]
        public void Setup()
        {
            _before = new GameState
            {
                Day = 4,
                Round = 5,
                Act = 67,
                TotalDays = 7,
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
                ScoringState = null,
                Clusters = new List<Cluster>()
                    { new Cluster() { Buildings = DefaultBuildingsList.Get() } },
                BuildingsInHand = DefaultBuildingsList.Get(),
                BuildingDeck = new BuildingDeck
                {
                    ShufflingRandom = Random.CreateFromIndex(342342),
                    Deck = DefaultBuildingsList.Get()
                },
                AvailableBuildings = DefaultBuildingsList.Get(),
                MaxBoosters = 34234,
                Boosters = BoosterList.Get(),
                AvailableBoosters = BoosterList.Get(),
                Shop = new ShopState
                {
                    StartingRerollCost = 324,
                    CurrentRerollCost = 23,
                    RerollIncreaseRate = 435,
                    CardCount = 3434,
                    ItemsForSale = new IPurchasableItem[]
                    {
                        new Consumable() { Name = "Consumable" },
                        new BoosterCard() { Name = "Booseter" },
                    },
                    BoosterSpawn = new Random[]
                    {
                        Random.CreateFromIndex(1),
                        Random.CreateFromIndex(2),
                        Random.CreateFromIndex(3),
                    },
                    BoosterAntiDuplicate = new Random[]
                    {
                        Random.CreateFromIndex(4),
                        Random.CreateFromIndex(5),
                        Random.CreateFromIndex(6),
                    },
                    CardPackSpawn = new Random[]
                    {
                        Random.CreateFromIndex(7),
                        Random.CreateFromIndex(8),
                        Random.CreateFromIndex(9678),
                    }
                },
                Result = GameResult.InProgress,
                WorldBoosters = new WorldBoostersState
                {
                    CountRandom = Random.CreateFromIndex(54),
                    SpawnRandom = Random.CreateFromIndex(54),
                    SelectionRandom = Random.CreateFromIndex(54),
                    PositionRandom = Random.CreateFromIndex(54),
                    SpawnChance = 23230,
                    Count = new MinMax(324, 3545),
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
            builder.RegisterModule<SerializationModule>();
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