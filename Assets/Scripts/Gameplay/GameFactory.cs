﻿using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public static class GameFactory
    {
        public static GameState NewGame(RogueRandom rogueRandom)
        {
            var seedRandom = rogueRandom.ForAct(0);
            var buildingBlueprints = DefaultBuildingsList.Get();
            var deck = DefaultBuildingsList.Get();
            deck.Shuffle(seedRandom);
            foreach (var building in deck)
            {
                building.Id = BuildingId.NewBuildingId();
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
                    ShufflingRandom = seedRandom.NextRandom(),
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
                DeckBuilding = new DeckBuildingState()
                {
                    BuildingSelectionRandom = seedRandom.NextRandom(),
                    AllConsumables = ConsumableList.Get(),
                },
                Shop = new ShopState
                {
                    StartingRerollCost = 5,
                    BoosterSpawn = seedRandom.NextRandom(),
                    CardPackSpawn = seedRandom.NextRandom(),
                    BoosterAntiDuplicate = seedRandom.NextRandom(),
                    SelectionRandom = seedRandom.NextRandom(),
                    CardCount = 3,
                    ItemsForSale = new IPurchasableItem[3],
                    CurrentRerollCost = 5,
                },
            };
        }

        public static RogueRandom NextRandom(this RandomForAct sysRandom)
            => new(sysRandom.NextUInt());

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