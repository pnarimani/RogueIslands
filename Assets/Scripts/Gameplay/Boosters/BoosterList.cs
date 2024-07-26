using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Descriptions;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters
{
    public static class BoosterList
    {
        public static List<BoosterCard> Get()
        {
            return new List<BoosterCard>
            {
                new()
                {
                    Name = "Simple",
                    Description = new LiteralDescription("x2 the score"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                        Multiplier = 2,
                    },
                },
                new()
                {
                    Name = "Sensitive",
                    Description = new LiteralDescription("All bonus scores count as normal triggers"),
                    BuyPrice = 10,
                },
                CreateColorBooster("Purple Boost", ColorTag.Purple),
                CreateColorBooster("Green Boost", ColorTag.Green),
                CreateColorBooster("Red Boost", ColorTag.Red),
                CreateColorBooster("Blue Boost", ColorTag.Blue),
                new()
                {
                    Name = "Network",
                    Description = new LiteralDescription("+100% range for all buildings"),
                    BuyPrice = 8,
                    BuyAction = new ModifyBuildingRangeAction
                    {
                        RangeMultiplier = 2f,
                        Source = new BuildingsInDeck(),
                    },
                    SellAction = new ModifyBuildingRangeAction
                    {
                        RangeMultiplier = 0.5f,
                        Source = new BuildingsInDeck(),
                    },
                    EventAction = new ModifyBuildingRangeAction
                    {
                        Conditions = new[] { GameEventCondition.Create<BuildingAdded>() },
                        RangeMultiplier = 2,
                        Source = new BuildingFromCurrentEvent(),
                    },
                },
                new()
                {
                    Name = "Sweatshop",
                    Description = new LiteralDescription("+20% for each red building in range"),
                    BuyPrice = 5,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                        Factor = new BuildingFromCurrentEvent()
                            .GetBuildingsInRange()
                            .WithCondition(new ColorCheckCondition { ForcedColors = new[] { ColorTag.Red } })
                            .Count(),
                        Multiplier = .2f,
                    },
                },
                new()
                {
                    Name = "Bad Eyesight",
                    Description =
                        new LiteralDescription(
                            $"{ColorTag.Red} and {ColorTag.Blue} count as the same. {ColorTag.Green} and {ColorTag.Purple} count as the same."),
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "Rigged",
                    Description = new LiteralDescription("Double all probabilities"),
                    BuyPrice = 4,
                },
                new()
                {
                    Name = "Saw Dust",
                    Description =
                        new ProbabilityDescription(
                            $"{{0}} to give x10 mult for each {Category.Cat3} building triggered"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat3 },
                            },
                            new ProbabilityCondition
                            {
                                FavorableOutcome = 1,
                                TotalOutcomes = 2,
                            },
                        },
                        Multiplier = 10,
                    },
                },
                new()
                {
                    Name = "Maizinator",
                    Description = new LiteralDescription($"{Category.Cat2} buildings trigger one more time."),
                    BuyPrice = 7,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterBuildingScoreTrigger>().Or<ResetRetriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat2 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Rotten Egg",
                    Description =
                        new LiteralDescription("-10% products, gains 5$ of sell value at the end of the round."),
                    BuyPrice = 2,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new GainSellValueAction
                            {
                                Amount = 5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEnd>(),
                                },
                            },
                            new ScoringAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<AfterAllBuildingTriggers>(),
                                },
                                Multiplier = 0.9,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Campfire",
                    Description = new ScalingBoosterDescription(
                        "Gains 0.5x mult for each booster sold. Resets at the end of the act."),
                    BuyPrice = 6,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<AfterAllBuildingTriggers>(),
                                },
                            },
                            new BoosterScalingAction
                            {
                                MultiplierChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BoosterSold>(),
                                },
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<ActEnd>(),
                                },
                            },
                        },
                    },
                },
                // new()
                // {
                //     Name = "Real-State Agent",
                //     Description = new LiteralDescription("For every 50 buildings placed, gain 1x mult."),
                //     BuyPrice = 2,
                // },
                new()
                {
                    Name = "Digger",
                    Description = new LiteralDescription("Pays $4 at the end of the round"),
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction
                    {
                        Conditions = new IGameCondition[] { GameEventCondition.Create<RoundEnd>() },
                        Change = 4,
                    },
                },
                new()
                {
                    Name = "Crowded",
                    Description = new ProbabilityDescription(
                        $"{{0}} chance to trigger {Category.Cat4} buildings 2 more times"),
                    BuyPrice = 6,
                    EventAction = new RetriggerBuildingAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterBuildingScoreTrigger>().Or<ResetRetriggers>(),
                            new BuildingCategoryCondition()
                            {
                                Categories = new[] { Category.Cat4 },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new ProbabilityCondition
                            {
                                FavorableOutcome = 1,
                                TotalOutcomes = 2,
                            },
                        },
                        RetriggerTimes = 2,
                    },
                },
                new()
                {
                    Name = "Painting",
                    Description =
                        new LiteralDescription("x4 score if at least one building of each color is placed down"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new ColorCheckCondition
                            {
                                Source = new PlacedDownBuildings(),
                                ForcedColors = ColorTag.All,
                            },
                        },
                        Multiplier = 4,
                    },
                },
                new()
                {
                    Name = "Binary World",
                    Description =
                        new LiteralDescription($"x20 mult if all buildings are {ColorTag.Green} or {ColorTag.Purple}"),
                    BuyPrice = 8,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new ColorCheckCondition
                            {
                                BannedColors = new[] { ColorTag.Red, ColorTag.Blue },
                                Source = new PlacedDownBuildings(),
                            },
                        },
                        Multiplier = 20,
                    },
                },
                new()
                {
                    Name = "Gennaro",
                    Description = new LiteralDescription("Sometimes maybe good..."),
                    BuyPrice = 3,
                    EventAction = new RandomScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                        },
                        Multiplier = 50,
                    },
                },
                new()
                {
                    Name = "Sacrifice",
                    Description = new LiteralDescription("x4 score if 6 or more buildings are placed down"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new CountCondition
                            {
                                Source = new PlacedDownBuildings(),
                                ComparisonMode = CountCondition.Mode.More,
                                Value = 5,
                            },
                        },
                        Multiplier = 4,
                    },
                },
                new()
                {
                    Name = "The Rat",
                    Description =
                        new ScalingBoosterDescription(
                            "On the start of the round, destroys a random booster. Gains 4x multiplier."),
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new RatAttack
                            {
                                Conditions = new[] { GameEventCondition.Create<RoundStart>() },
                            },
                            new ScoringAction
                            {
                                Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                                Multiplier = 0,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "The Collector",
                    Description = new LiteralDescription("+.25x score for each different building placed in the world"),
                    BuyPrice = 8,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                        Multiplier = .25,
                        Factor = new PlacedDownBuildings().Distinct().Count(),
                    },
                },
                new()
                {
                    Name = "The Banana",
                    Description =
                        new LiteralDescription("x15 score, 1 in 8 chance to get destroyed at the end of the round"),
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 15,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<AfterAllBuildingTriggers>(),
                                },
                            },
                            new DestroyBoosterAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEnd>(),
                                    new ProbabilityCondition
                                    {
                                        FavorableOutcome = 1,
                                        TotalOutcomes = 8,
                                    },
                                },
                                Self = true,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "City Steven",
                    Description = new LiteralDescription($"x5 score when {Category.Cat1} building is triggered"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat1 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Country Carl",
                    Description = new LiteralDescription($"+50 score when {Category.Cat2} building is triggered"),
                    BuyPrice = 4,
                    EventAction = new ScoringAction
                    {
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat2 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Ice Cream",
                    Description =
                        new ScalingBoosterDescription("+100 score, loses 5 score after each building is placed."),
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Products = 100,
                            },
                            new BoosterScalingAction
                            {
                                ProductChange = -5,
                            },
                            new DestroyBoosterAction
                            {
                                Conditions = new[] { new IsBoosterDepleted() },
                                Self = true,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Late Bloomer",
                    Description =
                        new ScalingBoosterDescription("Starts at 0.5x score. After 5 rounds, turns into x100 mult"),
                    BuyPrice = 3,
                    SellPrice = 2,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                                Multiplier = 0.5,
                            },
                            new BoosterScalingAction
                            {
                                MultiplierChange = 99.5,
                                Delay = 5,
                                OneTime = true,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEnd>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Stuntman",
                    Description = new LiteralDescription("+100 products when a building is placed down"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Products = 100,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlaced>(),
                        },
                    },
                },
                new()
                {
                    Name = "Capitalist",
                    Description =
                        new ScalingBoosterDescription("1x score for every $5 you have"),
                    BuyPrice = 7,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                        },
                        Multiplier = 1,
                        Factor = new MoneyAmount { DivideBy = 5 },
                    },
                },
                new()
                {
                    Name = "Bull",
                    Description =
                        new ScalingBoosterDescription("+10 score for every $2 you have when a building is placed down"),
                    BuyPrice = 7,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlaced>(),
                        },
                        Products = 10,
                        Factor = new MoneyAmount { DivideBy = 2 },
                    },
                },
                new()
                {
                    Name = "Gems",
                    Description = new LiteralDescription($"{Category.Cat4} buildings earn $1 when triggered"),
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction
                    {
                        Change = 1,
                        IsImmediate = true,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat4 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Popcorn",
                    Description = new ScalingBoosterDescription("4x, -1 mult per round played"),
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 4,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<AfterAllBuildingTriggers>(),
                                },
                            },
                            new BoosterScalingAction
                            {
                                MultiplierChange = -1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEnd>(),
                                },
                            },
                            new DestroyBoosterAction
                            {
                                Conditions = new[] { new IsBoosterDepleted() },
                                Self = true,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Explorer",
                    Description = new ScalingBoosterDescription("Gains 1x mult every time you reroll in the shop"),
                    BuyPrice = 8,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<AfterAllBuildingTriggers>(),
                                },
                            },
                            new BoosterScalingAction
                            {
                                MultiplierChange = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<ShopRerolledEvent>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Blueprint",
                    Description = new LiteralDescription("Copies the booster to the right"),
                    BuyPrice = 10,
                    EventAction = new CopyBoosterAction(),
                },
                new()
                {
                    Name = "Hug",
                    Description = new LiteralDescription("+0.8x for each Large building in range"),
                    BuyPrice = 4,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                        },
                        Factor = new BuildingsByRange
                            {
                                Center = new BuildingFromCurrentEvent(),
                                ReturnInRange = true,
                            }.WithCondition(new BuildingSizeCondition { Allowed = new[] { BuildingSize.Large } })
                            .Count(),
                        Multiplier = 0.8,
                    },
                },
                new()
                {
                    Name = "Sizing",
                    Description =
                        new LiteralDescription(
                            "x10 if there is at least one small, medium and large building in range"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                {
                                    Center = new BuildingFromCurrentEvent(),
                                    ReturnInRange = true,
                                }.WithCondition(new BuildingSizeCondition { Allowed = new[] { BuildingSize.Small } }),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                {
                                    Center = new BuildingFromCurrentEvent(),
                                    ReturnInRange = true,
                                }.WithCondition(new BuildingSizeCondition { Allowed = new[] { BuildingSize.Medium } }),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                {
                                    Center = new BuildingFromCurrentEvent(),
                                    ReturnInRange = true,
                                }.WithCondition(new BuildingSizeCondition { Allowed = new[] { BuildingSize.Large } }),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                        Multiplier = 10,
                    },
                },
                new()
                {
                    Name = "Away",
                    Description = new LiteralDescription("+0.3x for each small building out of range"),
                    BuyPrice = 5,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                        },
                        Factor = new BuildingFromCurrentEvent()
                            .GetBuildingsOutOfRange()
                            .WithCondition(new BuildingSizeCondition { Allowed = new[] { BuildingSize.Small } })
                            .Count(),
                        Multiplier = .3,
                    },
                },
                new()
                {
                    Name = "Quiet",
                    Description =
                        new LiteralDescription(
                            $"x10 score when a {Category.Cat1} building has no {Category.Cat3} building nearby"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 10,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat1 },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                {
                                    Center = new BuildingFromCurrentEvent(),
                                    ReturnInRange = true,
                                }.WithCondition(new BuildingCategoryCondition { Categories = new[] { Category.Cat3 } }),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.Equal,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Industry",
                    Description =
                        new LiteralDescription(
                            $"5x score when an {Category.Cat4} building has {Category.Cat3} buildings nearby"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat4 },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                {
                                    Center = new BuildingFromCurrentEvent(),
                                    ReturnInRange = true,
                                }.WithCondition(new BuildingCategoryCondition { Categories = new[] { Category.Cat3 } }),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Lowkey",
                    Description =
                        new LiteralDescription(
                            $"3x score when a placed down {Category.Cat5} building has no other {Category.Cat5} buildings nearby"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 3,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat5 },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                {
                                    Center = new BuildingFromCurrentEvent(),
                                    ReturnInRange = true,
                                }.WithCondition(new BuildingCategoryCondition { Categories = new[] { Category.Cat5 } }),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.Equal,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Colorful",
                    Description = new LiteralDescription("+200% same-color bonus score"),
                    BuyPrice = 3,
                    EventAction = new ModifyBonusAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonus>(),
                        },
                        ColorMultiplier = 2,
                    },
                },
                new()
                {
                    Name = "Good Year",
                    Description = new LiteralDescription("x5 all bonus scores"),
                    BuyPrice = 3,
                    EventAction = new ModifyBonusAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonus>(),
                        },
                        Multiplier = 5,
                    },
                },
                new()
                {
                    Name = "Tourism",
                    Description = new LiteralDescription($"x10 bonus score from {Category.Cat5} buildings"),
                    BuyPrice = 4,
                    EventAction = new ModifyBonusAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonus>(),
                            new BuildingCategoryCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Categories = new[] { Category.Cat5 },
                            },
                        },
                        Multiplier = 10,
                    },
                },
                new()
                {
                    Name = "Inside Out",
                    Description =
                        new LiteralDescription("Instead of in-range buildings, out of range buildings score bonus"),
                    BuyPrice = 4,
                },
                new()
                {
                    Name = "Double Trouble",
                    Description = new LiteralDescription("x2 score when a building scores bonus"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterBuildingBonus>(),
                        },
                    },
                },
                new()
                {
                    Name = "Mine Shaft",
                    Description = new LiteralDescription($"+50 score when an {Category.Cat4} building is triggered"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat4 },
                                Source = new BuildingFromCurrentEvent(),
                            },
                        },
                    },
                },
            };
        }

        private static BoosterCard CreateColorBooster(string name, ColorTag color)
        {
            return new BoosterCard
            {
                Name = name,
                Description = new LiteralDescription($"x3 score when {color} buildings get triggered"),
                BuyPrice = 3,
                EventAction = new ScoringAction
                {
                    Multiplier = 3,
                    Conditions = new IGameCondition[]
                    {
                        GameEventCondition.Create<BuildingTriggered>(),
                        new ColorCheckCondition
                        {
                            Source = new BuildingFromCurrentEvent(),
                            ForcedColors = new List<ColorTag> { color },
                        },
                    },
                },
            };
        }
    }
}