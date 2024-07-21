using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Descriptions;
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
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "Mr Producer",
                    Description = new LiteralDescription("Permanently add +2 product to placed buildings"),
                    BuyPrice = 4,
                    EventAction = new PermanentBuildingUpgradeAction
                    {
                        ProductUpgrade = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlaced>(),
                        },
                    },
                },
                CreateColorBooster("Purple Boost", ColorTag.Purple),
                CreateColorBooster("Green Boost", ColorTag.Green),
                CreateColorBooster("Red Boost", ColorTag.Red),
                CreateColorBooster("Blue Boost", ColorTag.Blue),
                new()
                {
                    Name = "Network",
                    Description = new LiteralDescription("+100% range for all buildings"),
                    BuyPrice = 5,
                    BuyAction = new ModifyBuildingRangeAction
                    {
                        RangeMultiplier = 2f,
                    },
                    SellAction = new ModifyBuildingRangeAction()
                    {
                        RangeMultiplier = 0.5f,
                    }
                },
                new()
                {
                    Name = "Sweatshop",
                    Description = new LiteralDescription("+20% output for each red building in range"),
                    BuyPrice = 5,
                    EventAction = new MultipliedScoringAction()
                    {
                        Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>(), },
                        MultiplyByRedBuildingsInRange = true,
                        Multiplier = 1.2f,
                    },
                },
                new()
                {
                    Name = "Bad Eyesight",
                    Description =
                        new LiteralDescription(
                            $"{ColorTag.Red} and {ColorTag.Blue} count as the same. {ColorTag.Green} and {ColorTag.Purple} count as the same."),
                },
                new()
                {
                    Name = "Rigged",
                    Description = new LiteralDescription("Double all probabilities"),
                },
                new()
                {
                    Name = "Saw Dust",
                    Description =
                        new ProbabilityDescription(
                            $"{{0}} to give x2 mult for each {Category.Cat3} building triggered"),
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
                        Multiplier = 2,
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
                            GameEventCondition.Create<AfterBuildingScoreTrigger>(),
                            BuildingTriggerCountCondition.FirstTrigger,
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat2 },
                            },
                        },
                    },
                },
                // new()
                // {
                //     Name = "Rotten Egg",
                //     Description =
                //         new LiteralDescription("-10% products, gains 5$ of sell value at the end of the round."),
                //     BuyPrice = 2,
                //     EventAction = new CompositeAction()
                //     {
                //         Actions = new GameAction[]
                //         {
                //             new GainSellValueAction
                //             {
                //                 Amount = 5,
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<RoundEnd>(),
                //                 },
                //             },
                //             new ScoringAction()
                //             {
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<BuildingPlaced>(),
                //                 },
                //                 Multiplier = 0.9,
                //             },
                //         },
                //     },
                // },
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
                // new()
                // {
                //     Name = "Crowded",
                //     Description = new ProbabilityDescription(
                //         $"{{0}} chance to retrigger all buildings when a {Category.Cat4} building is triggered."),
                //     BuyPrice = 6,
                //     EventAction = new RetriggerBuildingAction
                //     {
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<BuildingTriggered>(),
                //             BuildingTriggerCountCondition.FirstTrigger,
                //             new ProbabilityCondition()
                //             {
                //                 FavorableOutcome = 1,
                //                 TotalOutcomes = 4,
                //             },
                //         },
                //         RetriggerTimes = 1,
                //     },
                // },
                // new()
                // {
                //     Name = "Economy of Scale",
                //     Description = new LiteralDescription("+100 products if a cluster has 2 or less buildings"),
                //     BuyPrice = 8,
                //     EventAction = new ScoringAction
                //     {
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<ClusterScored>(),
                //             new CountCondition
                //             {
                //                 TargetType = CountCondition.Target.BuildingsInScoringIsland,
                //                 ComparisonMode = CountCondition.Mode.Less,
                //                 Value = 3,
                //             },
                //         },
                //         Products = 100,
                //     },
                // },
                new()
                {
                    Name = "Painting",
                    Description =
                        new LiteralDescription("x10 score if at least one building of each color is placed down"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new ColorCheckCondition
                            {
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
                        new LiteralDescription($"x30 mult if all buildings are {ColorTag.Green} or {ColorTag.Purple}"),
                    BuyPrice = 8,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new ColorCheckCondition
                            {
                                BannedColors = new[] { ColorTag.Red, ColorTag.Blue },
                            },
                        },
                        Multiplier = 30,
                    },
                },
                new()
                {
                    Name = "Gennaro",
                    Description = new LiteralDescription("Sometimes maybe good..."),
                    BuyPrice = 4,
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
                    Description = new LiteralDescription("x4 score if 6 or less buildings exist"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new CountCondition
                            {
                                TargetType = CountCondition.Target.Buildings,
                                ComparisonMode = CountCondition.Mode.Less,
                                Value = 7,
                            },
                        },
                        Multiplier = 20,
                    },
                },
                new()
                {
                    Name = "Big Hands",
                    Description = new LiteralDescription("+2 hand size"),
                    BuyPrice = 5,
                    BuyAction = new HandModifier
                    {
                        Change = 2,
                    },
                    SellAction = new HandModifier
                    {
                        Change = -2,
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
                            new RatAttack()
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
                    Description = new LiteralDescription("1.25x score for each different building placed in the world"),
                    BuyPrice = 8,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                        Multiplier = 1.25,
                        MultiplyByUniqueBuildings = true,
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
                // new()
                // {
                //     Name = "Super Banana",
                //     Description =
                //         new LiteralDescription("x4 mult, 1 in 1000 chance to get destroyed at the end of the round"),
                //     BuyPrice = 2,
                //     EventAction = new CompositeAction
                //     {
                //         Actions = new GameAction[]
                //         {
                //             new ScoringAction
                //             {
                //                 XMult = 4,
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<DayEnd>(),
                //                 },
                //             },
                //             new DestroyBoosterAction
                //             {
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<RoundEnd>(),
                //                     new ProbabilityCondition
                //                     {
                //                         FavorableOutcome = 1,
                //                         TotalOutcomes = 1000,
                //                     },
                //                 },
                //                 Self = true,
                //             },
                //         },
                //     },
                // },
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
                    BuyPrice = 5,
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
                // new()
                // {
                //     Name = "Lowkey Larry",
                //     Description = new LiteralDescription($"x3 mult for each {Category.Cat5} building remaining in hand"),
                //     BuyPrice = 5,
                //     EventAction = new ScoringAction
                //     {
                //         Multiplier = 4,
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<BuildingRemainedInHand>(),
                //             new BuildingCategoryCondition
                //             {
                //                 Categories = new[] { Category.Cat5 },
                //             },
                //         },
                //     },
                // },
                // new()
                // {
                //     Name = "Investment",
                //     Description = new LiteralDescription("$1 for each building remained in hand at the end of the day."),
                //     BuyPrice = 6,
                //     EventAction = new ChangeMoneyAction
                //     {
                //         IsImmediate = true,
                //         Change = 1,
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<BuildingRemainedInHand>(),
                //         },
                //     },
                // },
                new()
                {
                    Name = "Ice Cream",
                    Description =
                        new ScalingBoosterDescription("+20 score, loses 1 score after each building is placed."),
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
                            new DestroyBoosterAction()
                            {
                                Conditions = new[] { new IsBoosterDepleted() },
                                Self = true,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Normie",
                    Description = new LiteralDescription("x50 score for each identical building in range"),
                    BuyPrice = 6,
                    EventAction = new MultipliedScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                        },
                        Multiplier = 50,
                        MultiplyByIdenticalBuildingsInRange = true,
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
                            new ScoringAction()
                            {
                                Conditions = new[] { GameEventCondition.Create<AfterAllBuildingTriggers>() },
                                Multiplier = 0.5,
                            },
                            new BoosterScalingAction()
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
                    Description = new LiteralDescription("+250 products, -2 hand size"),
                    BuyPrice = 6,
                    BuyAction = new HandModifier
                    {
                        Change = -2,
                    },
                    SellAction = new HandModifier
                    {
                        Change = 2,
                    },
                    EventAction = new ScoringAction
                    {
                        Products = 250,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                        },
                    },
                },
                // new()
                // {
                //     Name = "Shoot the moon",
                //     Description =
                //         new LiteralDescription($"+10 mult for each {Category.Cat4} building remaining in hand"),
                //     BuyPrice = 5,
                //     EventAction = new ScoringAction
                //     {
                //         Multiplier = 10,
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<BuildingRemainedInHand>(),
                //             new BuildingCategoryCondition
                //             {
                //                 Categories = new[] { Category.Cat4 },
                //             },
                //         },
                //     },
                // },
                new()
                {
                    Name = "Capitalist",
                    Description =
                        new ScalingBoosterDescription("1x score for every $5 you have when a building is triggered"),
                    BuyPrice = 7,
                    EventAction = new MultipliedScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                        },
                        Multiplier = 2,
                        PerMoney = 5,
                    },
                },
                new()
                {
                    Name = "Bull",
                    Description =
                        new ScalingBoosterDescription("+10 score for every $2 you have when a building is placed down"),
                    BuyPrice = 7,
                    EventAction = new MultipliedScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlaced>(),
                        },
                        Products = 10,
                        PerMoney = 2,
                    },
                },
                // new()
                // {
                //     Name = "Rural",
                //     Description = new ScalingBoosterDescription(
                //         $"Gains 0.5x mult for every {Category.Cat1} building remained in hand. Resets at the end of the round"),
                //     BuyPrice = 5,
                //     EventAction = new CompositeAction
                //     {
                //         Actions = new GameAction[]
                //         {
                //             new ScoringAction
                //             {
                //                 XMult = 1,
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<DayEnd>(),
                //                 },
                //             },
                //             new BoosterScalingAction
                //             {
                //                 XMultChange = 0.5,
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<BuildingRemainedInHand>(),
                //                     new BuildingCategoryCondition
                //                     {
                //                         Categories = new[] { Category.Cat1 },
                //                     },
                //                 },
                //             },
                //             new BoosterResetAction
                //             {
                //                 XMult = 1,
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<RoundEnd>(),
                //                 },
                //             },
                //         },
                //     },
                // },
                new()
                {
                    Name = "Gems",
                    Description = new LiteralDescription($"{Category.Cat4} buildings earn $1 when triggered"),
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction()
                    {
                        Change = 1,
                        IsImmediate = true,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition()
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
                            new DestroyBoosterAction()
                            {
                                Conditions = new[] { new IsBoosterDepleted() },
                                Self = true,
                            },
                        },
                    },
                },
                // new()
                // {
                //     Name = "Juggler",
                //     Description = new LiteralDescription("+1 hand size"),
                //     BuyPrice = 4,
                //     EventAction = new HandModifier
                //     {
                //         Change = 1,
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<RoundStart>(),
                //         },
                //     },
                // }, 
                new()
                {
                    Name = "Explorer",
                    Description = new ScalingBoosterDescription("Gains 0.1x mult every time you reroll in the shop"),
                    BuyPrice = 8,
                    EventAction = new CompositeAction()
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
                                MultiplierChange = 0.1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<ShopRerolledEvent>(),
                                },
                            },
                        },
                    },
                },
                // new()
                // {
                //     Name = "Consolation Prize",
                //     Description = new LiteralDescription("+10 mult when a lucky event does not happen"),
                //     BuyPrice = 5,
                //     EventAction = new ScoringAction
                //     {
                //         PlusMult = 10,
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<ProbabilityConditionBlockedEvent>(),
                //         },
                //     },
                // },
                // new()
                // {
                //     Name = "Lucky Cat",
                //     Description = new ScalingBoosterDescription("Gains 0.5x mult when a lucky event happens"),
                //     BuyPrice = 5,
                //     EventAction = new CompositeAction()
                //     {
                //         Actions = new GameAction[]
                //         {
                //             new ScoringAction
                //             {
                //                 XMult = 1,
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<DayEnd>(),
                //                 },
                //             },
                //             new BoosterScalingAction
                //             {
                //                 XMultChange = 0.5,
                //                 Conditions = new IGameCondition[]
                //                 {
                //                     GameEventCondition.Create<BoosterScoredEvent>(), 
                //                     new ProbabilityBoosterScoredCondition(),
                //                 },
                //             },
                //         },
                //     },
                // },
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
                    Description = new LiteralDescription("x5 for each Large building in range when a small building gets triggered"),
                    BuyPrice = 4,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingSizeCondition
                            {
                                Allowed = new [] { BuildingSize.Small },
                            },
                        },
                        Multiplier = 5,
                        MultiplyByLargeBuildingsInRange = true,
                    },
                }
            };
        }

        private static BoosterCard CreateColorBooster(string name, ColorTag color)
        {
            return new BoosterCard
            {
                Name = name,
                Description = new LiteralDescription($"+3 mult when {color} buildings score"),
                BuyPrice = 3,
                EventAction = new ScoringAction
                {
                    Multiplier = 3,
                    Conditions = new IGameCondition[]
                    {
                        GameEventCondition.Create<BuildingTriggered>(),
                        new BuildingColorCondition
                        {
                            Colors = new[] { color },
                        },
                    },
                },
            };
        }
    }
}