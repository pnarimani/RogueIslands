using System;
using System.Collections.Generic;
using RogueIslands.Boosters;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;
using Random = Unity.Mathematics.Random;

namespace RogueIslands
{
    public static class GameFactory
    {
        public static GameState NewGame(System.Random seedRandom)
        {
            var buildingBlueprints = DefaultBuildingsList.Get();
            var deck = DefaultBuildingsList.Get();
            deck.Shuffle(seedRandom.NextRandom());
            foreach (var building in deck)
            {
                building.Id = new BuildingId((uint)Guid.NewGuid().GetHashCode());
            }

            const int handSize = 6;

            return new GameState()
            {
                AllRequiredScores = GetScoringRequirements(),
                CurrentEvent = new ActStart(),
                Buildings = new BuildingsState()
                {
                    All = buildingBlueprints,
                    Deck = deck,
                    ShufflingRandom = CreateRandomArray(seedRandom, GameState.TotalActs),
                },
                AvailableBoosters = BoosterList.Get(),
                HandSize = handSize,
                TotalDays = 4,
                WorldBoosters = new WorldBoostersState
                {
                    CountRandom = seedRandom.NextRandom(),
                    SpawnRandom = seedRandom.NextRandom(),
                    SelectionRandom = seedRandom.NextRandom(),
                    PositionRandom = seedRandom.NextRandom(),
                    SpawnChance = 0.2f,
                    Count = new MinMax(2, 6),
                    SpawnedBoosters = new List<WorldBooster>(),
                    All = WorldBoosterList.Get(),
                },
                Shop = new ShopState
                {
                    BoosterSpawn = seedRandom.CreateRandomArray(GameState.TotalActs),
                    CardPackSpawn = seedRandom.CreateRandomArray(GameState.TotalActs),
                    BoosterAntiDuplicate = seedRandom.CreateRandomArray(GameState.TotalActs),
                    CardCount = 2,
                    ItemsForSale = new IPurchasableItem[2],
                },
            };
        }

        public static Random NextRandom(this System.Random sysRandom)
            => new((uint)sysRandom.Next());

        public static Random[] CreateRandomArray(this System.Random sysRandom, int length)
        {
            var randoms = new Random[length];
            for (var i = 0; i < length; i++)
                randoms[i] = sysRandom.NextRandom();
            return randoms;
        }

        private static double[] GetScoringRequirements()
        {
            var result = new double[GameState.RoundsPerAct * GameState.TotalActs];
            for (var i = 0; i < GameState.TotalActs; i++)
            {
                for (var j = 0; j < GameState.RoundsPerAct; j++)
                {
                    var x = i * GameState.RoundsPerAct + j;
                    const double a = 47;
                    const double b = -52.5;
                    const double c = 112.5;
                    var score = a + b * x + c * x * x;

                    var digitCount = (int)Math.Log10(score) + 1;
                    var roundTo = Math.Pow(10, Math.Max(1, digitCount - 2));
                    score = Math.Round(score / roundTo) * roundTo;

                    result[x] = Math.Round(score);
                }
            }

            return result;
        }
    }
}