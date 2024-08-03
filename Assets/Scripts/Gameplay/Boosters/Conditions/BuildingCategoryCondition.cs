using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingCategoryCondition : IGameConditionWithSource<Building>
    {
        
        public IReadOnlyList<Category> Categories { get; set; }
        public ISource<Building> Source { get; set; }
        
        public BuildingCategoryCondition()
        {
        }

        public BuildingCategoryCondition(IReadOnlyList<Category> categories)
        {
            Categories = categories;
            Source = new BuildingFromCurrentEvent();
        }

        public BuildingCategoryCondition(ISource<Building> source, IReadOnlyList<Category> categories)
        {
            Source = source;
            Categories = categories;
        }

        public BuildingCategoryCondition(Category category)
        {
            Categories = new List<Category> { category };
            Source = new BuildingFromCurrentEvent();
        }

        public BuildingCategoryCondition(ISource<Building> source, Category category)
        {
            Source = source;
            Categories = new List<Category> { category };
        }
    }
}