using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public static class BoosterList
    {
        public static List<Booster> Get()
        {
            return new List<Booster>
            {
                new()
                {
                    Name = "Opps all sixes",
                    Description = "Add 1 to all probabilities",
                    BuyAction = new ProbabilityModifier() { Modification = 1 },
                    SellAction = new ProbabilityModifier() { Modification = -1 },
                    EventAction = new ProbabilityModifier()
                    {
                        Modification = 1,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("BoosterBought"),
                        },
                    },
                },
                new()
                {
                    Name = "Blood Pact",
                    Description = "1 in 2 chance to give x2 mult for each cat3 building scored",
                    EventAction = new ScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            new BuildingCategoryScoredCondition
                            {
                                Category = Category.Cat1,
                            },
                            new ProbabilityCondition
                            {
                                FavorableOutcome = 1,
                                TotalOutcomes = 2,
                            },
                            new GameEventCondition("BuildingScored"),
                        },
                        XMult = 2,
                    },
                },
                new()
                {
                    Name = "Hiker",
                    Description = "Permanently add +10 product to played buildings",
                    EventAction = new PermanentBuildingUpgradeAction()
                    {
                        ProductUpgrade = 10,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("BuildingScored"),
                        },
                    },
                },
                new()
                {
                    Name = "Suck my busking",
                    Description = "Retrigger all cat2 buildings",
                    EventAction = new RetriggerScoringBuildingAction()
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            new BuildingCategoryScoredCondition
                            {
                                Category = Category.Cat2,
                            },
                            new GameEventCondition("BuildingScored"),
                        },
                    },
                },
                new()
                {
                    Name = "Bad Eyesight",
                    Description = "Category 1 and 3 count as same. Category 2 and 4 count as same.",
                },
                new()
                {
                    Name = "Base 2",
                    Description = "If the number of buildings is a power of 2, gains +4 products",
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
                                ProductChange =   4,
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
                    EventAction = new ScoringAction()
                    {
                        Products = 30,
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("BuildingScored"),
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
                    Description = "You only have 1 day",
                    EventAction = new DayModifier
                    {
                        Conditions = new IGameCondition[]
                        {
                            new GameEventCondition("DayStart"),
                        },
                        SetDays = 1,
                    },
                },
                new()
                {
                    Name = "Stateful",
                    Description = "For every 50 buildings placed, gain 1x mult.",
                }
            };
        }
    }
}