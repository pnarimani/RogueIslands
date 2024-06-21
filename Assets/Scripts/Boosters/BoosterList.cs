using System.Collections.Generic;
using RogueIslands.Boosters.Descriptions;

namespace RogueIslands.Boosters
{
    public static class BoosterList
    {
        public static List<BoosterCard> Get(System.Random seedRandom)
        {
            return new List<BoosterCard>
            {
                new()
                {
                    Name = "Opps all sixes",
                    Description = new LiteralDescription("Add 1 to all probabilities"),
                    BuyPrice = 2,
                    EvaluationOverrides = new ConditionEvaluator[]
                    {
                        new ProbabilityEvaluator(seedRandom.NextRandom()),
                    },
                },
                new()
                {
                    Name = "Blood Pact",
                    Description =
                        new LiteralDescription(
                            $"1 in 2 chance to give x2 mult for each {Category.Cat3} building scored"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new SelectedBuildingCategory
                            {
                                Categories = new[] { Category.Cat1 },
                            },
                            new ProbabilityCondition
                            {
                                FavorableOutcome = 1,
                                TotalOutcomes = 2,
                            },
                        },
                        XMult = 2,
                    },
                },
                new()
                {
                    Name = "Hiker",
                    Description = new LiteralDescription("Permanently add +2 product to played buildings"),
                    BuyPrice = 2,
                    EventAction = new PermanentBuildingUpgradeAction()
                    {
                        ProductUpgrade = 2,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                        },
                    },
                },
                new()
                {
                    Name = "Suck my busking",
                    Description = new LiteralDescription($"Retrigger all `{Category.Cat2}` buildings"),
                    BuyPrice = 2,
                    EventAction = new RetriggerScoringBuildingAction()
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            BuildingTriggerCountCheck.FirstTrigger,
                            new SelectedBuildingCategory
                            {
                                Categories = new[] { Category.Cat2 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Bad Eyesight",
                    BuyPrice = 2,
                    Description =
                        new LiteralDescription($"{ColorTag.Red} and {ColorTag.Blue} are the same. {ColorTag.White} and {ColorTag.Black} are the same."),
                    EvaluationOverrides = new[] { new BadEyesConditionEvaluator() },
                },
                new()
                {
                    Name = "Base 2",
                    Description =
                        new ScalingBoosterDescription("If the number of buildings is a power of 2, gains +4 products")
                            { ShowProducts = true },
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
                    {
                        Actions = new GameAction[]
                        {
                            new BoosterScalingAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("DayStart"),
                                    new CountCondition
                                    {
                                        TargetType = CountCondition.Target.Buildings,
                                        ComparisonMode = CountCondition.Mode.PowerOfTwo,
                                    },
                                },
                                ProductChange = 4,
                            },
                            new ScoringAction
                            {
                                Products = 0,
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("DayEnd"),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Egg",
                    Description = new LiteralDescription("Gains 3$ of sell value at the end of every week"),
                    BuyPrice = 2,
                    EventAction = new GainSellValueAction
                    {
                        Amount = 3,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("WeekEnd"),
                        },
                    },
                },
                new()
                {
                    Name = "Campfire",
                    Description =
                        new LiteralDescription(
                            "Gains 0.5x mult for each booster sold. Resets at the end of the month."),
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction()
                            {
                                XMult = 1,
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("DayEnd"),
                                },
                            },
                            new BoosterScalingAction()
                            {
                                XMultChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("BoosterSold"),
                                },
                            },
                            new BoosterResetAction()
                            {
                                XMult = 1,
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("MonthEnd"),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Clutch",
                    Description = new LiteralDescription("+10 mult on the last day."),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        PlusMult = 10,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                            TimeCondition.LastDay(),
                        },
                    },
                },
                new()
                {
                    Name = "Overtime",
                    Description = new LiteralDescription("On the last day, Retrigger all buildings"),
                    BuyPrice = 2,
                    EventAction = new RetriggerScoringBuildingAction()
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            BuildingTriggerCountCheck.FirstTrigger,
                            TimeCondition.LastDay(),
                        },
                    },
                },
                new()
                {
                    Name = "Sweatshop",
                    Description = new LiteralDescription("+30 products for each red building"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        Products = 30,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new SelectedBuildingColorCondition()
                            {
                                Colors = new[] { ColorTag.Red },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Procrastinator",
                    Description = new LiteralDescription("x3 mult. you only have 1 day"),
                    BuyPrice = 2,
                    BuyAction = new DayModifier { SetDays = 1 },
                    SellAction = new DayModifier { ShouldSetToDefault = true },
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new DayModifier
                            {
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition(new[]
                                        { "DayStart", "DayEnd", "BoosterBought", "BoosterSold" }),
                                },
                                SetDays = 1,
                            },
                            new ScoringAction
                            {
                                Conditions = new[] { new GameEventCondition("DayEnd") },
                                XMult = 3,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Real-State Agent",
                    Description = new LiteralDescription("For every 50 buildings placed, gain 1x mult."),
                    BuyPrice = 2,
                },
                new()
                {
                    Name = "Digger",
                    Description = new LiteralDescription("Pays $4 at the end of the round"),
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction()
                    {
                        Conditions = new IGameCondition[] { new GameEventCondition("WeekEnd") },
                        Change = 4,
                    },
                },
                CreateColorBooster("Black Boost", "+3 mult when black buildings score", ColorTag.Black),
                CreateColorBooster("White Boost", "+3 mult when white buildings score", ColorTag.White),
                CreateColorBooster("Red Boost", "+3 mult when red buildings score", ColorTag.Red),
                CreateColorBooster("Blue Boost", "+3 mult when blue buildings score", ColorTag.Blue),
                new()
                {
                    Name = "Crowded",
                    Description = new LiteralDescription("Retrigger all buildings if there are exactly 5 groups"),
                    BuyPrice = 2,
                    EventAction = new RetriggerScoringBuildingAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            BuildingTriggerCountCheck.FirstTrigger,
                            new CountCondition
                            {
                                TargetType = CountCondition.Target.Island,
                                ComparisonMode = CountCondition.Mode.Equal,
                                Value = 5,
                            },
                        },
                        RetriggerTimes = 1,
                    },
                },
                new()
                {
                    Name = "Economy of Scale",
                    Description = new LiteralDescription("+100 products if an group has 2 or less buildings"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new CountCondition
                            {
                                TargetType = CountCondition.Target.BuildingsInScoringIsland,
                                ComparisonMode = CountCondition.Mode.Less,
                                Value = 3,
                            },
                        },
                        Products = 100,
                    },
                },
                new()
                {
                    Name = "Diversity",
                    Description = new LiteralDescription("+10 mult if all colors are present"),
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                            new ColorCheckCondition
                            {
                                ColorsToExist = ColorTag.All,
                            },
                        },
                        PlusMult = 10,
                    },
                },
                new()
                {
                    Name = "Binary World",
                    Description = new LiteralDescription("x3 mult if all buildings are black and white"),
                    BuyPrice = 3,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                            new ColorCheckCondition()
                            {
                                ColorsToExist = new[] { ColorTag.White, ColorTag.Black },
                                ColorsToNotExist = new[] { ColorTag.Red, ColorTag.Blue },
                            },
                        },
                        XMult = 3,
                    },
                },
                new()
                {
                    Name = "10X",
                    Description = new LiteralDescription("Apparently is worth 10 times more than other boosters"),
                    BuyPrice = 2,
                    EventAction = new RandomScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                        },
                        PlusMult = 50,
                    },
                },
                new()
                {
                    Name = "Simple",
                    Description = new LiteralDescription("+4 mult"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        Conditions = new[] { new GameEventCondition("DayEnd") },
                        PlusMult = 4,
                    },
                },
                new()
                {
                    Name = "Sacrifice",
                    Description = new LiteralDescription("+20 mult if 6 or less buildings exist"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                            new CountCondition
                            {
                                TargetType = CountCondition.Target.Buildings,
                                ComparisonMode = CountCondition.Mode.Less,
                                Value = 7,
                            },
                        },
                        PlusMult = 20,
                    },
                },
                new()
                {
                    Name = "Big Hands",
                    Description = new LiteralDescription("+2 hand size, -1 day"),
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
                    {
                        Conditions = new[] { new GameEventCondition("WeekStart") },
                        Actions = new GameAction[]
                        {
                            new DayModifier
                            {
                                Change = -1,
                            },
                            new HandModifier()
                            {
                                Change = 2,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "The Rat",
                    Description =
                        new ScalingBoosterDescription(
                            "On the start of the week, destroys the booster to the right. Gains +5 mult")
                        {
                            ShowPlusMult = true,
                        },
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
                    {
                        Actions = new GameAction[]
                        {
                            new CompositeAction()
                            {
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("WeekStart"),
                                },
                                Actions = new GameAction[]
                                {
                                    new DestroyBoosterAction()
                                    {
                                    },
                                    new BoosterScalingAction()
                                    {
                                        PlusMultChange = 5,
                                    },
                                },
                            },
                            new ScoringAction()
                            {
                                Conditions = new[] { new GameEventCondition("DayEnd") },
                            }
                        }
                    },
                },
                new()
                {
                    Name = "Efficiency",
                    Description = new LiteralDescription("+50 products for each day remaining"),
                    BuyPrice = 2,
                    EventAction = new MultipliedScoringAction()
                    {
                        MultiplyByDay = true,
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                        },
                    },
                },
                new()
                {
                    Name = "The Collector",
                    Description = new LiteralDescription("+1 mult for each different building"),
                    BuyPrice = 2,
                },
                new()
                {
                    Name = "Raised Fist United",
                    Description = new LiteralDescription("add double the number of groups to the mult"),
                    BuyPrice = 2,
                    EventAction = new MultipliedScoringAction()
                    {
                        MultiplyByIslandCount = true,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                        },
                        PlusMult = 1,
                    },
                },
                new()
                {
                    Name = "The Banana",
                    Description =
                        new LiteralDescription("+15 mult, 1 in 8 chance to get destroyed at the end of the week"),
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction()
                            {
                                PlusMult = 15,
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("DayEnd"),
                                },
                            },
                            new DestroyBoosterAction()
                            {
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("WeekEnd"),
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
                    Name = "Super Banana",
                    Description =
                        new LiteralDescription("x4 mult, 1 in 1000 chance to get destroyed at the end of the week"),
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction()
                            {
                                XMult = 4,
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("DayEnd"),
                                },
                            },
                            new DestroyBoosterAction()
                            {
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("WeekEnd"),
                                    new ProbabilityCondition
                                    {
                                        FavorableOutcome = 1,
                                        TotalOutcomes = 1000,
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
                    Description = new LiteralDescription($"+4 mult for each {Category.Cat1} building"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        PlusMult = 4,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new SelectedBuildingCategory
                            {
                                Categories = new[] { Category.Cat1 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Country Carl",
                    Description = new LiteralDescription($"+50 products for each {Category.Cat2} building"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new SelectedBuildingCategory
                            {
                                Categories = new[] { Category.Cat2 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Lowkey Larry",
                    Description =
                        new LiteralDescription($"+4 mult for each {Category.Cat5} building remaining in hand"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        PlusMult = 4,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("BuildingRemainedInHand"),
                            new SelectedBuildingCategory()
                            {
                                Categories = new[] { Category.Cat5 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Investment",
                    Description = new LiteralDescription("$1 for each building remained in hand"),
                    BuyPrice = 2,
                    EventAction = new ChangeMoneyAction()
                    {
                        IsImmediate = true,
                        Change = 1,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("BuildingRemainedInHand"),
                        },
                    },
                },
                new()
                {
                    Name = "Ice Cream",
                    Description = new ScalingBoosterDescription("+100 products, loses 5 products every day.")
                        { ShowProducts = true },
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
                    {
                        Conditions = new[] { new GameEventCondition("DayEnd") },
                        Actions = new GameAction[]
                        {
                            new ScoringAction()
                            {
                                Products = 100,
                            },
                            new BoosterScalingAction()
                            {
                                ProductChange = -5,
                            }
                        },
                    },
                },
                new()
                {
                    Name = "Normie",
                    Description = new LiteralDescription("x5 mult if all buildings are the same"),
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                            new SameBuildingsCondition(),
                        }
                    }
                }
            };
        }

        private static BoosterCard CreateColorBooster(string name, string desc, ColorTag color)
            => new()
            {
                Name = name,
                Description = new LiteralDescription(desc),
                BuyPrice = 2,
                EventAction = new ScoringAction()
                {
                    PlusMult = 3,
                    Conditions = new IGameCondition[]
                    {
                        new GameEventCondition("AfterBuildingScored"),
                        new SelectedBuildingColorCondition()
                        {
                            Colors = new[] { color },
                        },
                    },
                },
            };
    }
}