using System.Collections.Generic;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public static class WorldBoosterList
    {
        public static List<WorldBooster> Get()
        {
            return new List<WorldBooster>()
            {
                new()
                {
                    Name = "Simple Product Boost",
                    Description = new LiteralDescription("+5 products"),
                    Range = 4,
                    PrefabAddress = "WorldBoosters/Simple",
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new SelectedBuildingRangeCondition(),
                        },
                        Products = 5,
                    },
                },
                new()
                {
                    Name = "Tree",
                    Description = new LiteralDescription($"+10 products for all {Category.Cat3} buildings"),
                    Range = 2,
                    PrefabAddress = "WorldBoosters/Tree",
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new SelectedBuildingRangeCondition(),
                            new SelectedBuildingCategory() { Category = Category.Cat3 },
                        },
                        Products = 10,
                    },
                }
            };
        }
    }
}