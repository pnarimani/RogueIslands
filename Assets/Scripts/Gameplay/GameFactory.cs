﻿using System;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.Rand;
using UnityEngine;

namespace RogueIslands.Gameplay
{
    public static class GameFactory
    {
        public static GameState NewGame(SeedRandom seedRandom)
        {
            const int handSize = 6;

            var buildingBlueprints = DefaultBuildingsList.Get();
            var deck = DefaultBuildingsList.Get();
            deck.Shuffle(seedRandom);
            deck = deck.GetRange(0, handSize);
            foreach (var building in deck)
            {
                building.Id = BuildingId.NewBuildingId();
            }


            var allBoosters = BoosterList.Get();

            Debug.Log("Booster Count = " + allBoosters.Count);

            return new GameState()
            {
                SeedRandom = seedRandom,
                AllRequiredScores = GetScoringRequirements(),
                CurrentEvent = new ActStart(),
                Buildings = new BuildingsState()
                {
                    All = buildingBlueprints,
                    Deck = deck,
                    ShufflingRandom = seedRandom.NextRandom(),
                },
                AvailableBoosters = allBoosters,
                HandSize = handSize,
                CardPackSelectionRandom = seedRandom.NextRandom(),
                CardSelectionRandom = seedRandom.NextRandom(),
                Consumables = new ConsumablesState()
                {
                    BuildingSelectionRandom = seedRandom.NextRandom(),
                    AllConsumables = ConsumableList.Get(),
                },
                Shop = new ShopState
                {
                    StartingRerollCost = 5,
                    BoosterSpawn = seedRandom.NextRandom(),
                    CardPackSpawn = seedRandom.NextRandom(),
                    DeduplicationRandom = seedRandom.NextRandom(),
                    BuildingSpawn = seedRandom.NextRandom(),
                    SelectionRandom = seedRandom.NextRandom(),
                    CurrentRerollCost = 5,
                },
            };
        }



        private static double[] GetScoringRequirements()
        {
            var reqPerAct = new double[]
            {
                20,
                600,
                3000,
                25000,
            };

            var result = new double[GameState.RoundsPerAct * GameState.TotalActs];
            for (var i = 0; i < GameState.TotalActs; i++)
            {
                for (var j = 0; j < GameState.RoundsPerAct; j++)
                {
                    var x = i * GameState.RoundsPerAct + j;

                    var score = reqPerAct[i] * (j + 1);

                    result[x] = Math.Round(score);
                }
            }

            return result;
        }
    }
}