using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters
{
    public static class WorldBoosterList
    {
        public static List<WorldBooster> Get()
        {
            return null;
            // return new List<WorldBooster>()
            // {
            //     new()
            //     {
            //         Name = "Tree",
            //         Description = new LiteralDescription($"+10 products for all {Category.Cat3} buildings"),
            //         Range = 12,
            //         PrefabAddress = "WorldBoosters/Tree",
            //         EventAction = new ScoringAction
            //         {
            //             Conditions = new IGameCondition[]
            //             {
            //                 GameEventCondition.Create<BuildingPlaced>(),
            //                 new BuildingInRangeCondition(),
            //                 new BuildingCategoryCondition() { Categories = new[] { Category.Cat3 } },
            //             },
            //             Products = 10,
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Mine",
            //         Description =
            //             new LiteralDescription($"+3 mult for all {Category.Cat4} and {Category.Cat5} buildings"),
            //         Range = 10,
            //         PrefabAddress = "WorldBoosters/Mine",
            //         EventAction = new ScoringAction
            //         {
            //             Conditions = new IGameCondition[]
            //             {
            //                 GameEventCondition.Create<BuildingPlaced>(),
            //                 new BuildingInRangeCondition(),
            //                 new BuildingCategoryCondition() { Categories = new[] { Category.Cat4, Category.Cat5, } },
            //             },
            //             Multiplier = 3,
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Money Tree",
            //         Description = new LiteralDescription("$1 for every building in range"),
            //         Range = 8,
            //         PrefabAddress = "WorldBoosters/MoneyTree",
            //         EventAction = new ChangeMoneyAction
            //         {
            //             Conditions = new IGameCondition[]
            //             {
            //                 GameEventCondition.Create<BuildingPlaced>(),
            //                 new BuildingInRangeCondition(),
            //             },
            //             Change = 1,
            //             IsImmediate = true,
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Shopping Mall",
            //         Description = new LiteralDescription($"+10 mult for {Category.Cat1} buildings in range"),
            //         Range = 16,
            //         PrefabAddress = "WorldBoosters/ShoppingMall",
            //         EventAction = new ScoringAction
            //         {
            //             Conditions = new IGameCondition[]
            //             {
            //                 GameEventCondition.Create<BuildingPlaced>(),
            //                 new BuildingInRangeCondition(),
            //                 new BuildingCategoryCondition() { Categories = new[] { Category.Cat1 } },
            //             },
            //             Multiplier = 10,
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Museum",
            //         Description =
            //             new ScalingBoosterDescription("Starts at -50 products, +20 for each building in range"),
            //         Range = 15,
            //         PrefabAddress = "WorldBoosters/Museum",
            //         EventAction = new CompositeAction()
            //         {
            //             Actions = new GameAction[]
            //             {
            //                 new ScoringAction()
            //                 {
            //                     Conditions = new IGameCondition[]
            //                     {
            //                         GameEventCondition.Create<DayEnd>(),
            //                     },
            //                 },
            //                 new WorldBoosterScalingAction()
            //                 {
            //                     StartingProducts = -50,
            //                     ProductChangePerBuildingInside = 20,
            //                 },
            //             },
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Protected Park",
            //         Description = new ScalingBoosterDescription("Starts at +30 mult, -5 for each building in range"),
            //         Range = 2 * 10,
            //         PrefabAddress = "WorldBoosters/Park",
            //         EventAction = new CompositeAction()
            //         {
            //             Actions = new GameAction[]
            //             {
            //                 new ScoringAction()
            //                 {
            //                     Conditions = new IGameCondition[]
            //                     {
            //                         GameEventCondition.Create<DayEnd>(),
            //                     },
            //                 },
            //                 new WorldBoosterScalingAction()
            //                 {
            //                     StartingMultiplier = 30,
            //                     PlusMultChangePerBuildingInside = -5,
            //                 },
            //             },
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Factory",
            //         Description = new LiteralDescription("When a building is placed in range, double its output"),
            //         Range = 10,
            //         PrefabAddress = "WorldBoosters/Factory",
            //         EventAction = new ModifyBuildingOutputAction
            //         {
            //             Conditions = new IGameCondition[]
            //             {
            //                 GameEventCondition.Create<BuildingPlaced>(),
            //                 new BuildingInRangeCondition(),
            //             },
            //             ProductMultiplier = 2,
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Garbage Dump",
            //         Description = new ScalingBoosterDescription("+1 mult for each building out of range"),
            //         Range = 2 * 10,
            //         PrefabAddress = "WorldBoosters/GarbageDump",
            //         EventAction = new CompositeAction()
            //         {
            //             Actions = new GameAction[]
            //             {
            //                 new ScoringAction()
            //                 {
            //                     Conditions = new IGameCondition[]
            //                     {
            //                         GameEventCondition.Create<DayEnd>(),
            //                     },
            //                 },
            //                 new WorldBoosterScalingAction()
            //                 {
            //                     StartingMultiplier = 0,
            //                     PlusMultChangePerBuildingOutside = 1,
            //                 },
            //             },
            //         },
            //     },
            //     new()
            //     {
            //         Name = "Lava",
            //         Description =
            //             new LiteralDescription(
            //                 "Permanently destroys all buildings in range and then destroys itself at the end of the day."),
            //         Range = 2 * 5,
            //         PrefabAddress = "WorldBoosters/Lava",
            //         EventAction = new CompositeAction()
            //         {
            //             Conditions = new IGameCondition[]
            //             {
            //                 GameEventCondition.Create<DayEnd>(),
            //             },
            //             Actions = new GameAction[]
            //             {
            //                 new DestroyBuildingsAction
            //                 {
            //                     DestroyBuildingsInRange = true,
            //                 },
            //                 new DestroyBoosterAction()
            //                 {
            //                     Self = true,
            //                 },
            //             }
            //         },
            //     },
            // };
        }
    }
}