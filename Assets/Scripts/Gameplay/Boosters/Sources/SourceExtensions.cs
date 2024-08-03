using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public static class SourceExtensions
    {
        public static ISource<int> Count<T>(this ISource<T> source) => new CountItems<T> { Source = source };

        public static BuildingsWithCondition With(this ISource<Building> source,
            IGameConditionWithSource<Building> condition) => new() { Source = source, Condition = condition };

        public static BuildingsWithCondition WithCategory(this ISource<Building> source, Category category)
            => source.With(new BuildingCategoryCondition { Categories = new[] { category } });

        public static BuildingsWithCondition WithSize(this ISource<Building> source, BuildingSize size)
            => source.With(new BuildingSizeCondition { Allowed = new[] { size } });

        public static BuildingsWithCondition WithColor(this ISource<Building> source, ColorTag color) 
            => source.With(new ColorCheckCondition { ForcedColors = new[] { color } });

        public static BuildingsByRange GetBuildingsInRange(this ISource<Building> source) =>
            new() { Center = source, ReturnInRange = true };

        public static BuildingsByRange GetBuildingsOutOfRange(this ISource<Building> source) =>
            new() { Center = source, ReturnInRange = false };

        public static ISource<Building> Distinct(this ISource<Building> source) =>
            new DistinctBuildings { Source = source };

        public static ISource<ColorTag> GetColors(this ISource<Building> source) =>
            new GetBuildingColors { Source = source };

        public static ISource<BuildingSize> GetSizes(this ISource<Building> source) =>
            new GetBuildingSizes { Source = source };

        public static ISource<int> GetPlayCountThisRound(this ISource<BuildingSize> source) =>
            new SizePlayCountSource { Size = source };
    }
}