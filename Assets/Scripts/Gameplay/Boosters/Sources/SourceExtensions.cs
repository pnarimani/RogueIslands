using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public static class SourceExtensions
    {
        public static ISource<int> Count<T>(this ISource<T> source)
        {
            return new CountItems<T> { Source = source };
        }

        public static BuildingsWithCondition WithCondition(this ISource<Building> source, IGameConditionWithSource<Building> condition)
        {
            return new BuildingsWithCondition { Source = source, Condition = condition };
        }

        public static BuildingsByRange GetBuildingsInRange(this ISource<Building> source)
        {
            return new BuildingsByRange { Center = source, ReturnInRange = true };
        }
        
        public static BuildingsByRange GetBuildingsOutOfRange(this ISource<Building> source)
        {
            return new BuildingsByRange { Center = source, ReturnInRange = false };
        }
        
        public static ISource<Building> Distinct(this ISource<Building> source)
        {
            return new DistinctBuildings { Source = source };
        }
    }
}