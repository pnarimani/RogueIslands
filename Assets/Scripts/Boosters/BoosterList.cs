﻿using System.Collections.Generic;
using RogueIslands.Boosters.Descriptions;

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
                    Description =
                        new LiteralDescription(
                            $"{Category.Cat1} and {Category.Cat3} count as same. Category {Category.Cat2} and {Category.Cat4} count as same."),
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
                                }
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
                    Description = new LiteralDescription("On the last day, +10 mult"),
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
                            new GameEventCondition("BuildingFirstTrigger"),
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
                            new BuildingColorScoredCondition()
                            {
                                ColorTag = ColorTag.Red,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Procrastinator",
                    Description = new LiteralDescription("x3 mult. you only have 1 day"),
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
            };
        }
    }
}