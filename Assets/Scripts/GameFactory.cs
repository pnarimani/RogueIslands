using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public class GameFactory
    {
        public static GameState NewGame()
        {
            List<Building> buildings = BuildingsList.Get();
            buildings.Shuffle();
            return new GameState()
            {
                AllRequiredScores = new double[GameState.TotalWeeks * GameState.TotalMonths],
                CurrentEvent = "MonthStart",
                AvailableBuildings = buildings,
                AvailableBoosters = BoosterList.Get(),
                BuildingsInHand = buildings.Take(4).ToList(),
            };
        }
    }
}