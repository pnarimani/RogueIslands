using System.Linq;
using RogueIslands.Boosters;
using Random = Unity.Mathematics.Random;

namespace RogueIslands
{
    public static class GameFactory
    {
        public static GameState NewGame(System.Random seedRandom)
        {
            var buildings = DefaultBuildingsList.GetDefaultDeckBuildings();
            buildings.Shuffle(seedRandom);

            const int handSize = 6;
            
            return new GameState()
            {
                AllRequiredScores = GetScoringRequirements(),
                CurrentEvent = "MonthStart",
                AvailableBuildings = buildings,
                BuildingDeck = buildings.ToList(),
                AvailableBoosters = BoosterList.Get(seedRandom),
                HandSize = handSize,
                DefaultHandSize = handSize,
                BuildingsInHand = buildings.Take(handSize).ToList(),
                TotalDays = 4,
                Shop = new ShopState
                {
                    BoosterSpawn = seedRandom.CreateRandomArray(GameState.TotalMonths),
                    CardPackSpawn = seedRandom.CreateRandomArray(GameState.TotalMonths),
                    BoosterAntiDuplicate = seedRandom.CreateRandomArray(GameState.TotalMonths),
                    CardCount = 2,
                    ItemsForSale = new Booster[2],
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
            var result = new double[GameState.TotalWeeks * GameState.TotalMonths];
            for (var i = 0; i < GameState.TotalMonths; i++)
            {
                result[i * GameState.TotalWeeks + 0] = (i + 1) * 10;
                result[i * GameState.TotalWeeks + 1] = (i + 1) * 25;
                result[i * GameState.TotalWeeks + 2] = (i + 1) * 50;
                result[i * GameState.TotalWeeks + 3] = (i + 1) * 100;

            }
            return result;
        }
    }
}