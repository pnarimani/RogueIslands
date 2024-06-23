using System.Collections.Generic;
using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Descriptions;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;

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
                    Name = "Tree",
                    Description = new LiteralDescription($"+10 products for all {Category.Cat3} buildings"),
                    Range = 6,
                    PrefabAddress = "WorldBoosters/Tree",
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            new BuildingInRangeCondition(),
                            new SelectedBuildingCategory() { Categories = new[] { Category.Cat3 } },
                        },
                        Products = 10,
                    },
                },
                new()
                {
                    Name = "Mine",
                    Description = new LiteralDescription($"+3 mult for all {Category.Cat4} and {Category.Cat5} buildings"),
                    Range = 4,
                    PrefabAddress = "WorldBoosters/Mine",
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            new BuildingInRangeCondition(),
                            new SelectedBuildingCategory() { Categories = new[] { Category.Cat4, Category.Cat5, } },
                        },
                        PlusMult = 3,
                    },
                },
                new()
                {
                    Name = "Money Tree",
                    Description = new LiteralDescription("$1 for every building in range"),
                    Range = 4,
                    PrefabAddress = "WorldBoosters/MoneyTree",
                    EventAction = new ChangeMoneyAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            new BuildingInRangeCondition(),
                        },
                        Change = 1,
                        IsImmediate = true,
                    },
                },
            };
        }
    }
}