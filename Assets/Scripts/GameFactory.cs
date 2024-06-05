using System.Linq;
using RogueIslands.Boosters;
using Random = Unity.Mathematics.Random;

namespace RogueIslands
{
    public static class GameFactory
    {
        public static GameState NewGame(string seed)
        {
            var random = new System.Random(seed.GetHashCode());
            
            var buildings = DefaultBuildingsList.Get();
            buildings.Shuffle(random);
            
            return new GameState()
            {
                AllRequiredScores = GetScoringRequirements(),
                CurrentEvent = "MonthStart",
                AvailableBuildings = buildings,
                AvailableBoosters = BoosterList.Get(),
                BuildingsInHand = buildings.Take(4).ToList(),
                Shop = new ShopState
                {
                    BoosterSpawn = random.CreateRandomArray(GameState.TotalMonths),
                    CardPackSpawn = random.CreateRandomArray(GameState.TotalMonths),
                    BoosterAntiDuplicate = random.CreateRandomArray(GameState.TotalMonths),
                    CardCount = 2,
                    BoostersForSale = new Booster[2],
                },
            };
        }

        private static Random NextRandom(this System.Random sysRandom) 
            => new((uint)sysRandom.Next());

        private static Random[] CreateRandomArray(this System.Random sysRandom, int length)
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