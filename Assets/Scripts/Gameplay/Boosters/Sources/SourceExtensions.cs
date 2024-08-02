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

        public static BuildingsWithCondition With(this ISource<Building> source, IGameConditionWithSource<Building> condition)
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

        public static ISource<ColorTag> GetColors(this ISource<Building> source)
        {
            return new GetBuildingColors() { Source = source };
        }
        
        public static ISource<BuildingSize> GetSizes(this ISource<Building> source)
        {
            return new GetBuildingSizes() { Source = source };
        }
        
        public static ISource<int> GetPlayCountThisRound(this ISource<BuildingSize> source)
        {
            return new SizePlayCountSource() { Size = source };
        }
    }
}