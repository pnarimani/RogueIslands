using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using Random = Unity.Mathematics.Random;

namespace RogueIslands
{
    public class GameFactory
    {
        public static GameState NewGame()
        {
            var buildings = DefaultBuildingsList.Get();
            buildings.Shuffle();
            return new GameState()
            {
                AllRequiredScores = new double[GameState.TotalWeeks * GameState.TotalMonths],
                CurrentEvent = "MonthStart",
                AvailableBuildings = buildings,
                AvailableBoosters = BoosterList.Get(),
                BuildingsInHand = buildings.Take(4).ToList(),
                Shop = new ShopState
                {
                    BoosterSpawn = Enumerable.Range(0, GameState.TotalMonths)
                        .Select(month => new Random((uint)month))
                        .ToArray(),
                    CardPackSpawn = Enumerable.Range(0, GameState.TotalMonths)
                        .Select(month => new Random((uint)month))
                        .ToArray(),
                    BoosterAntiDuplicate = Enumerable.Range(0, GameState.TotalMonths)
                        .Select(month => new Random((uint)month))
                        .ToArray(),
                    CardCount = 2,
                },
            };
        }
    }
}