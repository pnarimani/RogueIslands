﻿using System.Linq;
using RogueIslands.Boosters;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;
using RogueIslands.Rollback;
using Random = Unity.Mathematics.Random;

namespace RogueIslands
{
    public static class GameFactory
    {
        public static GameState NewGame(System.Random seedRandom)
        {
            var buildings = DefaultBuildingsList.GetDefaultDeckBuildings();
            buildings.Shuffle(seedRandom.NextRandom());

            const int handSize = 6;

            return new GameState()
            {
                AllRequiredScores = GetScoringRequirements(),
                CurrentEvent = new ActStart(),
                AvailableBuildings = buildings,
                BuildingDeck = new BuildingDeck
                {
                    Deck = buildings,
                    ShufflingRandom = seedRandom.NextRandom(),
                },
                AvailableBoosters = BoosterList.Get(seedRandom),
                HandSize = handSize,
                AvailableWorldBoosters = WorldBoosterList.Get(),
                WorldBoosterSpawnRandom = seedRandom.NextRandom(),
                WorldBoosterSelectionRandom = seedRandom.NextRandom(),
                TotalDays = 4,
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
                result[i * GameState.RoundsPerAct + 0] = i * 1200 + (i + 1) * 30;
                result[i * GameState.RoundsPerAct + 1] = i * 1200 + (i + 1) * 100;
                result[i * GameState.RoundsPerAct + 2] = i * 1200 + (i + 1) * 400;
                result[i * GameState.RoundsPerAct + 3] = i * 1200 + (i + 1) * 1000;
            }

            return result;
        }
    }
}