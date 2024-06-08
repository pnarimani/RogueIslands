using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public static class BoosterList
    {
        public static List<Booster> Get(System.Random seedRandom)
        {
            return new List<Booster>
            {
                new()
                {
                    Name = "Opps all sixes",
                    Description = "Add 1 to all probabilities",
                    BuyPrice = 2,
                    EvaluationOverrides = new ConditionEvaluator[]
                    {
                        new ProbabilityEvaluator(seedRandom.NextRandom()),
                    },
                },
                new()
                {
                    Name = "Blood Pact",
                    Description = "1 in 2 chance to give x2 mult for each cat3 building scored",
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new BuildingCategoryScoredCondition
                            {
                                Category = Category.Cat1,
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
                    Description = "Permanently add +2 product to played buildings",
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
                    Description = "Retrigger all cat2 buildings",
                    BuyPrice = 2,
                    EventAction = new RetriggerScoringBuildingAction()
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("BuildingFirstTrigger"),
                            new BuildingCategoryScoredCondition
                            {
                                Category = Category.Cat2,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Bad Eyesight",
                    BuyPrice = 2,
                    Description = "Category 1 and 3 count as same. Category 2 and 4 count as same.",
                    EvaluationOverrides = new[] { new BadEyesConditionEvaluator() },
                },
                new()
                {
                    Name = "Base 2",
                    Description = "If the number of buildings is a power of 2, gains +4 products",
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
                                }
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Egg",
                    Description = "Gains 3$ of sell value at the end of every week",
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
                    Description = "Gains 0.5x mult for each booster sold. Resets at the end of the month.",
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
                                }
                            },
                            new BoosterScalingAction()
                            {
                                XMultChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    new GameEventCondition("BoosterSold"),
                                }
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
                    Description = "On the last day, +10 mult",
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
                    Description = "On the last day, Retrigger all buildings",
                    BuyPrice = 2,
                    EventAction = new RetriggerScoringBuildingAction()
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayEnd"),
                            TimeCondition.LastDay(),
                        },
                    },
                },
                new()
                {
                    Name = "Sweatshop",
                    Description = "+30 products for each red building",
                    BuyPrice = 2,
                    EventAction = new ScoringAction()
                    {
                        Products = 30,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("AfterBuildingScored"),
                            new BuildingCategoryScoredCondition
                            {
                                Category = Category.Cat3,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Procrastinator",
                    Description = "x3 mult. you only have 1 day",
                    BuyPrice = 2,
                    BuyAction = new DayModifier { SetDays = 1, },
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
                                        { "DayStart", "DayEnd", "BoosterBought", "BoosterSold", })
                                },
                                SetDays = 1,
                            },
                            new ScoringAction
                            {
                                Conditions = new[] { new GameEventCondition("DayEnd") },
                                XMult = 3,
                            },
                        },
                    }
                },
                new()
                {
                    Name = "Realstate Agent",
                    Description = "For every 50 buildings placed, gain 1x mult.",
                    BuyPrice = 2,
                }
            };
        }
    }
}