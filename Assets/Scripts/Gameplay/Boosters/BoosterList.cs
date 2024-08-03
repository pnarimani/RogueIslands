using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using static RogueIslands.Gameplay.Boosters.Rarity;
using static RogueIslands.Gameplay.Buildings.BuildingSize;
using static RogueIslands.Gameplay.Buildings.Category;
using static RogueIslands.Gameplay.Buildings.ColorTag;

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
                    Description = "{score} to score",
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                        Addition = 30,
                    },
                },
                new()
                {
                    Name = "Sensitive",
                    Description = "All <e>bonuses</e> count as <e>triggers</e>",
                    BuyPrice = 10,
                    Rarity = Rare,
                },
                CreateColorBooster("Purple Boost", Purple),
                CreateColorBooster("Green Boost", Green),
                CreateColorBooster("Red Boost", Red),
                CreateColorBooster("Blue Boost", Blue),
                CreateCategoryBooster("Citizen", City),
                CreateCategoryBooster("Cow", Farming),
                CreateCategoryBooster("Hammer", Iron),
                CreateCategoryBooster("Country Carl", Lumber),
                CreateCategoryBooster("Sculpture", Statue),
                new()
                {
                    Name = "Network",
                    Description = "X2 the range for all buildings",
                    Rarity = Rare,
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
                        Condition = new GameEventCondition<BuildingAddedEvent>(),
                        RangeMultiplier = 2,
                        Source = new BuildingFromCurrentEvent(),
                    },
                },
                new()
                {
                    Name = "Sweatshop",
                    Description = "{mult} for each {red} building in range",
                    Rarity = Uncommon,
                    BuyPrice = 5,
                    EventAction = new MultipliedScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                        Factor = new BuildingFromCurrentEvent()
                            .GetBuildingsInRange()
                            .With(new ColorCheckCondition(Red))
                            .Count(),
                        Multiplier = .2f,
                    },
                },
                new()
                {
                    Name = "Bad Eyesight",
                    Description = "{Red} and {Blue} count as the same.\n{Green} and {Purple} count as the same",
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "Rigged",
                    Description = "Double all probabilities\n<p>(ex: 1 in 3 -> 2 in 3)",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                },
                new()
                {
                    Name = "Saw Dust",
                    Description = "{probability} to {mult} the score\n when a {lumber} building <e>scores bonus",
                    BuyPrice = 8,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Condition = new GameEventCondition<AfterBuildingBonusEvent>()
                            .And(new BuildingCategoryCondition(Lumber))
                            .And(new ProbabilityCondition(1, 2)),
                        Multiplier = 2,
                    },
                },
                new()
                {
                    Name = "Maizinator",
                    Description = "{farming} buildings <e>trigger</e> one more time",
                    BuyPrice = 6,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 1,
                        Condition = new GameEventCondition<RetriggerStepEvent>()
                            .And(new BuildingCategoryCondition(Farming)),
                    }.And(new ResetRetriggersAction()),
                },
                new()
                {
                    Name = "Rotten Egg",
                    Description = "Gains <d>$3</d> sell value \nat the end of the round",
                    BuyPrice = 2,
                    EventAction = new GainSellValueAction
                    {
                        Amount = 2,
                        Condition = new GameEventCondition<RoundEndEvent>(),
                    },
                },
                new()
                {
                    Name = "Campfire",
                    Description = "{score} the score.\n" +
                                  "Gains {multchange} for each booster sold.\n" +
                                  "Resets at the end of the act",
                    BuyPrice = 6,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    }.And(new ScoreScalingAction
                    {
                        MultiplierChange = 0.5,
                        Condition = new GameEventCondition<BoosterSoldEvent>(),
                    }.And(new BoosterResetAction
                    {
                        Multiplier = 1,
                        Condition = new GameEventCondition<ActEndEvent>(),
                    })),
                },
                new()
                {
                    Name = "RealState Agent",
                    Description = "{score} the score\n" +
                                  "For every 8 buildings placed,\n" +
                                  "gains {multchange} mult",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    }.And(new ScoreScalingAction
                    {
                        MultiplierChange = 0.2,
                        Delay = 8,
                        OneTime = false,
                        Condition = new GameEventCondition<BuildingPlacedEvent>(),
                    }),
                },
                new()
                {
                    Name = "Digger",
                    Description = "Pays {money} at the end of the round",
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction
                    {
                        Condition = new GameEventCondition<RoundEndEvent>(),
                        Change = 4,
                    },
                },
                new()
                {
                    Name = "Ukulele",
                    Description = "{probability} to <e>trigger</e>\n {iron} buildings 2 more times",
                    BuyPrice = 6,
                    EventAction = new RetriggerBuildingAction
                    {
                        Condition = new GameEventCondition<RetriggerStepEvent>()
                            .And(new BuildingCategoryCondition(Iron))
                            .And(new ProbabilityCondition(1, 2)),
                        RetriggerTimes = 2,
                    }.And(new ResetRetriggersAction()),
                },
                new()
                {
                    Name = "Painting",
                    Description = "{mult} the score\n" +
                                  "if at least one building of each color\n" +
                                  " is placed down",
                    BuyPrice = 8,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>()
                            .And(new ColorCheckCondition
                            {
                                Source = new PlacedDownBuildings(),
                                ForcedColors = ColorTag.All,
                            }),
                        Multiplier = 2,
                    },
                },
                new()
                {
                    Name = "Binary World",
                    Description = "{mult} the score\nif all buildings are {green} or {purple}",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>()
                            .And(new ColorCheckCondition
                            {
                                BannedColors = new[] { Red, Blue },
                                Source = new PlacedDownBuildings(),
                            }),
                        Multiplier = 3,
                    },
                },
                new()
                {
                    Name = "Gennaro",
                    Description = "...",
                    BuyPrice = 5,
                    EventAction = new RandomScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                        Multiplier = 2.5,
                    },
                },
                new()
                {
                    Name = "The Rat",
                    Description = "On the start of the round,\ndestroys a random booster.\nGains <m>x1</m> multiplier.",
                    BuyPrice = 8,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new RatAttack
                            {
                                Change = 1,
                                Condition = new GameEventCondition<RoundStartEvent>(),
                            },
                            new ScoringAction
                            {
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                                Multiplier = 0,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "The Collector",
                    Description = "<m>X0.05</m> score for each different\n building placed in the world",
                    BuyPrice = 8,
                    Rarity = Uncommon,
                    EventAction = new MultipliedScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                        Multiplier = .05,
                        Factor = new PlacedDownBuildings().Distinct().Count(),
                    },
                },
                new()
                {
                    Name = "The Banana",
                    Description = "{score} score\n {probability} to get destroyed\n at the end of the round",
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 150,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new DestroyBoosterAction
                            {
                                Condition = new GameEventCondition<RoundEndEvent>()
                                    .And(new ProbabilityCondition(1, 6)),
                                Self = true,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "City Steven",
                    Description = "{score} the score when the played building is {city}\n" +
                                  "Gains {multchange} when a {city} building is in range",
                    BuyPrice = 8,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.1,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>()
                            .And(new BuildingCategoryCondition(City)),
                    }.And(new ScoreScalingAction
                    {
                        MultiplierChange = 0.05,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new CountCondition
                            {
                                ComparisonMode = CountCondition.Mode.More,
                                Value = 0,
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(City)
                                    .Count(),
                            }),
                    }),
                },
                new()
                {
                    Name = "Late Bloomer",
                    Description = "Starts at <m>X0.5</m> score.\n After 5 rounds, it turns into <m>X2</m> mult",
                    BuyPrice = 4,
                    Rarity = Common,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                                Multiplier = 0.5,
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 1.5,
                                Delay = 5,
                                OneTime = true,
                                Condition = new GameEventCondition<RoundEndEvent>(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Stuntman",
                    Description = "{add} when a building <e>is placed down",
                    BuyPrice = 3,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Addition = 35,
                        Condition = new GameEventCondition<BuildingPlacedEvent>(),
                    },
                },
                new()
                {
                    Name = "Capitalist",
                    Description = "{score} for every <d>$2</d> you have.",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new MultipliedScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                        Addition = 5,
                        Factor = new MoneyAmount { DivideBy = 2 },
                    },
                },
                new()
                {
                    Name = "Gems",
                    Description = "Earn {money} when an {iron} building <e>is triggered",
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction
                    {
                        Change = 1,
                        IsImmediate = true,
                        Condition = new GameEventCondition<BuildingTriggeredEvent>()
                            .And(new BuildingCategoryCondition(Iron)),
                    },
                },
                new()
                {
                    Name = "Popcorn",
                    Description = "Starts with {score} mult.\n{multchange} mult per round played",
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 2,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = -0.25,
                                Condition = new GameEventCondition<RoundEndEvent>(),
                            },
                            new DestroyBoosterAction
                            {
                                Condition = new IsBoosterDepleted(),
                                Self = true,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Explorer",
                    Description = "{score} the score.\n\nGains {multchange} every time you <e>reroll</e> in the shop",
                    BuyPrice = 8,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 0,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.05,
                                Condition = new GameEventCondition<ShopRerolledEvent>(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Blueprint",
                    Description = "Copies the booster to the right",
                    BuyPrice = 10,
                    Rarity = Rare,
                    EventAction = new CopyBoosterAction(),
                },
                new()
                {
                    Name = "Hug",
                    Description = "{score} the score\n" +
                                  "Gains {multchange} when there is\n" +
                                  "a {large} building in range",
                    BuyPrice = 9,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                                {
                                    ComparisonMode = CountCondition.Mode.More,
                                    Value = 0,
                                    Source = new BuildingFromCurrentEvent()
                                        .GetBuildingsInRange()
                                        .WithSize(Large)
                                        .Count(),
                                }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Sizing",
                    Description = "{score} the score\n" +
                                  "if there is at least one {small},\n" +
                                  " {medium} and {large} building in range",
                    BuyPrice = 6,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>()
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithSize(Small)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            })
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithSize(Medium)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            })
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithSize(Large)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }),
                        Multiplier = 2,
                    },
                },
                new()
                {
                    Name = "Away",
                    Description = "{score} the score.\n" +
                                  "Gains {multchange} when there is\n" +
                                  "a {small} building out of range",
                    BuyPrice = 9,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.05,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                                {
                                    ComparisonMode = CountCondition.Mode.More,
                                    Value = 0,
                                    Source = new BuildingFromCurrentEvent()
                                        .GetBuildingsOutOfRange()
                                        .WithSize(Small)
                                        .Count(),
                                }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Quiet",
                    Description = "{score} when the played building is {city}.\n\n" +
                                  "Gains extra {addchange} when a {lumber}\n" +
                                  " building is out of range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition(City),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 30,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 10,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsOutOfRange()
                                            .With(new BuildingCategoryCondition
                                                { Categories = new[] { Lumber } })
                                            .Count(),
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Industry",
                    Description = "{score} the score if the played building is {iron}\n" +
                                  "Gains {multchange} if a {lumber} building is in range",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition(Lumber),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1.5,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .WithCategory(Lumber)
                                            .Count(),
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Lowkey",
                    Description = "{score} the score when the played building is {statue}\n" +
                                  "Gains extra {multchange} when there is a {statue} out of range",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition
                        {
                            Categories = new[] { Statue },
                            Source = new BuildingFromCurrentEvent(),
                        },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1.5,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                                {
                                    ComparisonMode = CountCondition.Mode.More,
                                    Value = 0,
                                    Source = new BuildingFromCurrentEvent()
                                        .GetBuildingsOutOfRange()
                                        .WithCategory(Statue)
                                        .Count(),
                                }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Colorful",
                    Description = "{mult} <e>color bonus",
                    BuyPrice = 3,
                    Rarity = Common,
                    EventAction = new BonusAction
                    {
                        Condition = new GameEventCondition<BuildingBonusEvent>(),
                        ColorMultiplier = 2,
                    },
                },
                new()
                {
                    Name = "Good Year",
                    Description = "{score} <e>bonus</e>.\n\n" +
                                  "Gains <a>1</a> more bonus for\n" +
                                  "every 10 buildings that score bonus.",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new BonusAction
                            {
                                Addition = 5,
                                Condition = new GameEventCondition<BuildingBonusEvent>(),
                            },
                            new BonusScalingAction
                            {
                                AdditionChange = 1,
                                Delay = 10,
                                OneTime = false,
                                Condition = new GameEventCondition<BuildingBonusEvent>(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Tourism",
                    Description = "{score} <e>bonus</e> score from {Statue} buildings",
                    BuyPrice = 4,
                    Rarity = Common,
                    EventAction = new BonusAction
                    {
                        Condition = new GameEventCondition<BuildingBonusEvent>()
                            .And(new BuildingCategoryCondition(Statue)),
                        Multiplier = 2,
                    },
                },
                new()
                {
                    Name = "Inside Out",
                    Description = "Instead of in-range buildings,\n out of range buildings score <e>bonus</e>",
                    BuyPrice = 10,
                    Rarity = Rare,
                },
                new()
                {
                    Name = "Bonus Money",
                    Description = "{money} when a building <e>scores bonus</e>",
                    BuyPrice = 6,
                    Rarity = Rare,
                    EventAction = new ChangeMoneyAction
                    {
                        IsImmediate = true,
                        Change = 1,
                        Condition = new GameEventCondition<AfterBuildingBonusEvent>(),
                    },
                },
                new()
                {
                    Name = "Mine Shaft",
                    Description = "{score} score when an {Iron} building <e>is triggered</e>.\n\n" +
                                  "Gains extra {addchange} when a {Lumber} building is in of range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Addition = 20,
                        Condition = new GameEventCondition<BuildingTriggeredEvent>()
                            .And(new BuildingCategoryCondition(Iron)),
                    }.And(new ScoreScalingAction
                    {
                        AdditionChange = 5,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new CountCondition
                            {
                                ComparisonMode = CountCondition.Mode.More,
                                Value = 0,
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(Lumber)
                                    .Count(),
                            }),
                    }),
                },
                new()
                {
                    Name = "The Wall",
                    Description = "{score} the score.\n\n" +
                                  "Gains {multchange} mult \n" +
                                  "when a {medium} or {large} building <e>is placed</e>.\n" +
                                  "Resets when a {small} building <e>is placed.",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    }.And(new ScoreScalingAction
                    {
                        MultiplierChange = 0.2,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new BuildingSizeCondition { Allowed = new[] { Medium, Large } }),
                    }).And(new BoosterResetAction
                    {
                        Multiplier = 1,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new BuildingSizeCondition(Small)),
                    }),
                },
                new()
                {
                    Name = "Black Coat",
                    Description = "{score} score when you have <d>$6</d> or less",
                    Rarity = Uncommon,
                    BuyPrice = 7,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 4,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>().And(new CountCondition
                        {
                            Source = new MoneyAmount(),
                            Value = 7,
                            ComparisonMode = CountCondition.Mode.Less,
                        }),
                    },
                },
                new()
                {
                    Name = "Decorations",
                    Description = "{score} score when the played building is {statue}.\n\n" +
                                  "Gains extra {addchange} when a {city} building is in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Addition = 30,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    }.And(new ScoreScalingAction
                    {
                        AdditionChange = 8,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new CountCondition
                            {
                                ComparisonMode = CountCondition.Mode.More,
                                Value = 0,
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(City)
                                    .Count(),
                            }),
                    }).When(new BuildingCategoryCondition(Statue)),
                },
                new()
                {
                    Name = "More Interest",
                    Description = "Earn <d>$1</d> more in interest\n per <d>$5</d> in the bank.",
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "Food",
                    Description = "{score} when a {large} building is played.\n\n" +
                                  "Gains extra {addchange} when the building\n" +
                                  " is also {farming}.",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Addition = 15,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    }.And(new ScoreScalingAction
                    {
                        AdditionChange = 15,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new BuildingCategoryCondition(Farming)),
                    }).When(new BuildingSizeCondition(Large)),
                },
                new()
                {
                    Name = "Free Reroll",
                    Description = "Get a free <e>reroll</e> in every shop",
                    BuyPrice = 4,
                },
                new()
                {
                    Name = "Last Breath",
                    Description = "After you played your last building\nRetrigger all buildings",
                    BuyPrice = 8,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        Condition = new GameEventCondition<RetriggerStepEvent>()
                            .And(new CountCondition
                            {
                                ComparisonMode = CountCondition.Mode.Equal,
                                Value = 0,
                                Source = new BuildingsInDeck().Count(),
                            }),
                    }.And(new ResetRetriggersAction()),
                },
                new()
                {
                    Name = "Blue Jeans",
                    Description = "{score} <e>bonus</e> from {blue} buildings." +
                                  "\nGains <a>+1</a> when a {blue} building <e>is placed</e>." +
                                  "\nLoses <a>1</a> when a {red} building <e>is placed.",
                    BuyPrice = 4,
                    EventAction = new BonusAction
                    {
                        Addition = 10,
                        Condition = new GameEventCondition<BuildingBonusEvent>()
                            .And(new ColorCheckCondition(Blue)),
                    }.And(new BonusScalingAction
                    {
                        AdditionChange = 1,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new ColorCheckCondition(Blue)),
                    }).And(new BonusScalingAction
                    {
                        AdditionChange = -1,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new ColorCheckCondition(Red)),
                    }),
                },
                new()
                {
                    Name = "Modular",
                    Description = "{score} score when a {small} building <e>is placed",
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Addition = 50,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new BuildingSizeCondition(Small)),
                    },
                },
                new()
                {
                    Name = "Restraint",
                    Description = "Every round, a card is\n" +
                                  "selected from your hand.\n " +
                                  "If you don't play that card,\n" +
                                  "earn {money} at the end of the round",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new SelectCardAction().When(new GameEventCondition<RoundStartEvent>())
                        .And(new ChangeMoneyAction
                        {
                            Change = 10,
                            IsImmediate = false,
                            Condition = new GameEventCondition<RoundEndEvent>()
                                .And(new SelectedCardNotPlayedCondition()),
                        }),
                },
                new()
                {
                    Name = "Mid",
                    Description = "{score} score when a {medium} building <e>is placed",
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Addition = 50,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new BuildingSizeCondition(Medium)),
                    },
                },
                // new()
                // {
                //     Name = "Subsidized",
                //     Description = "Prevents loss\n if your score is at least \n<a>%25</a> of the required score.\nSelf destructs when activated.",
                //     BuyPrice = 4,
                // },
                new()
                {
                    Name = "Extra Time",
                    Description = "Retrigger the next 8 buildings",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        Condition = new GameEventCondition<RetriggerStepEvent>(),
                        RetriggerTimes = 1,
                        RemainingCharges = 8,
                    }.And(new ResetRetriggersAction()),
                },
                new()
                {
                    Name = "Swasher",
                    Description = "Adds double the sell value\n of all owned boosters to the score",
                    BuyPrice = 4,
                    EventAction = new MultipliedScoringAction
                    {
                        Addition = 2,
                        Factor = new SellPrice<BoosterCard>
                        {
                            Source = new OwnedBoosters(),
                        },
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    },
                },
                new()
                {
                    Name = "Erosion",
                    Description = "{score} the score.\n" +
                                  "Loses {multchange} when a building <e>is placed</e>.\n" +
                                  "Resets when the round starts.",
                    BuyPrice = 8,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 2,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = -0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>(),
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 1,
                                Condition = new GameEventCondition<RoundStartEvent>(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Freebie",
                    Description = "Buildings in the shop are <d>$1</d> cheaper",
                    BuyPrice = 4,
                    Rarity = Rare,
                },
                new()
                {
                    Name = "Hold Off",
                    Description = "<m>X0.1</m> score \nfor every {statue} building in hand",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new MultipliedScoringAction
                    {
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                        Factor = new BuildingsInHand()
                            .WithCategory(Statue)
                            .Count(),
                        Multiplier = 0.1,
                    },
                },
                new()
                {
                    Name = "Bigger is Better",
                    Description = "{score} score when a {large} building <e>is placed",
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Addition = 70,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new BuildingSizeCondition(Large)),
                    },
                },
                new()
                {
                    Name = "State",
                    Description = "{score} score when a {city} \n" +
                                  "building <e>is triggered</e>\n" +
                                  "Gains extra {addchange} when \n" +
                                  "a {city} building <e>scores bonus</e>",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 20,
                                Condition = new GameEventCondition<BuildingTriggeredEvent>()
                                    .And(new BuildingCategoryCondition(City)),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 5,
                                Condition = new GameEventCondition<AfterBuildingBonusEvent>()
                                    .And(new BuildingCategoryCondition(City)),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Bronze Lion",
                    Description = "When the played building\n" +
                                  " is {statue}, {score} to score.\n\n" +
                                  "Gains extra {addchange} when an\n" +
                                  "{iron} building in range.",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition(Statue),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 30,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 8,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                                {
                                    ComparisonMode = CountCondition.Mode.More,
                                    Value = 0,
                                    Source = new BuildingFromCurrentEvent()
                                        .GetBuildingsInRange()
                                        .WithCategory(Iron)
                                        .Count(),
                                }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Perimeter",
                    Description = "{score} score when a {lumber} building <e>is triggered</e>.\n\n" +
                                  "Gains extra {addchange} when there is \n" +
                                  "a {lumber} building out of range",
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 30,
                                Condition = new GameEventCondition<BuildingTriggeredEvent>()
                                    .And(new BuildingCategoryCondition(Lumber)),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 10,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsOutOfRange()
                                            .WithCategory(Lumber)
                                            .Count(),
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Plot",
                    Description = "{score} score when a {farming}\n" +
                                  " building <e>is triggered</e>.\n\n" +
                                  "Gains {addchange} when another {farming}\n" +
                                  "building is in range",
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 20,
                                Condition = new GameEventCondition<BuildingTriggeredEvent>()
                                    .And(new BuildingCategoryCondition(Farming)),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 5,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .WithCategory(Farming)
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Tetros",
                    Description = "{score} score when a {medium} building <e>is placed</e>.\n\n" +
                                  "Gains extra {addchange} when a {large} building is in range",
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 10,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new BuildingSizeCondition(Medium)),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 5,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .WithSize(Large)
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Tritanopia",
                    Description = "{score} score when there are\n {blue} buildings out of range",
                    BuyPrice = 3,
                    EventAction = new ScoringAction
                    {
                        Addition = 100,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>()
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsOutOfRange()
                                    .WithColor(Blue)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }),
                    },
                },
                new()
                {
                    Name = "Corporate Love",
                    Description = "Earn <d>$1</d> for every \n{green} building in range",
                    BuyPrice = 5,
                    EventAction = new ChangeMoneyAction
                    {
                        Change = 1,
                        IsImmediate = true,
                        Multiplier = new BuildingFromCurrentEvent()
                            .GetBuildingsInRange()
                            .WithColor(Green)
                            .Count(),
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    },
                },
                new()
                {
                    Name = "Purple Heart",
                    Description = "{score} score when a building <e>is placed</e>.\n\n" +
                                  "Gains extra {addchange} when there is\n" +
                                  "a {purple} building out of range",
                    BuyPrice = 3,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 10,
                                Condition = new GameEventCondition<BuildingPlacedEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 2,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                                {
                                    Source = new BuildingFromCurrentEvent()
                                        .GetBuildingsOutOfRange()
                                        .WithColor(Purple)
                                        .Count(),
                                    Value = 0,
                                    ComparisonMode = CountCondition.Mode.More,
                                }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Si",
                    Description = "When a {small} building scores <e>bonus</e>\n {score} score",
                    BuyPrice = 10,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.5,
                        Condition = new GameEventCondition<AfterBuildingBonusEvent>()
                            .And(new BuildingSizeCondition(Small)),
                    },
                },
                new()
                {
                    Name = "Edge",
                    Description = "Gains <m>X0.5</m> mult\n" +
                                  "for every {large} building placed.\n" +
                                  "Resets at the end of the round",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.5,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new BuildingSizeCondition(Large)),
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 1,
                                Condition = new GameEventCondition<RoundEndEvent>(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Overtime",
                    Description = "{score} the score when the played\n" +
                                  "building is {large}\n" +
                                  "Gains {multchange} when the building\n" +
                                  "is also {lumber}",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingSizeCondition(Large),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1.5,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new BuildingCategoryCondition(Lumber)),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Eco",
                    Description = "{probability} to earn {money}\n when a {small} building <e>is triggered",
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction
                    {
                        Change = 1,
                        IsImmediate = true,
                        Condition = new GameEventCondition<BuildingTriggeredEvent>()
                            .And(new BuildingSizeCondition(Small))
                            .And(new ProbabilityCondition(1, 2)),
                    },
                },
                new()
                {
                    Name = "Middle Ground",
                    Description = "{score} score when a {medium} building <e>is triggered",
                    BuyPrice = 4,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.5,
                        Condition = new GameEventCondition<BuildingTriggeredEvent>()
                            .And(new BuildingSizeCondition(Medium)),
                    },
                },
                new()
                {
                    Name = "Debut",
                    Description = "Every round, the first building\n" +
                                  "of each size scores {score} <e>when placed</e>.\n" +
                                  "Gains extra {addchange} when the act is completed",
                    BuyPrice = 3,
                    Rarity = Common,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 100,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                                {
                                    ComparisonMode = CountCondition.Mode.Equal,
                                    Value = 0,
                                    Source = new BuildingFromCurrentEvent()
                                        .GetSizes()
                                        .GetPlayCountThisRound(),
                                }),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 50,
                                Condition = new GameEventCondition<ActEndEvent>(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Smith",
                    Description = "{score} the score\n" +
                                  "{probability} to gain {multchange} " +
                                  "when a building <e>is placed",
                    BuyPrice = 7,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1.5,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new ProbabilityCondition
                                {
                                    FavorableOutcome = 1,
                                    TotalOutcomes = 2,
                                }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Watch",
                    Description = "{score} the score when the played building is {farming}\n" +
                                  "Gains {multchange} when there is a {lumber} building out of range",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition(Farming),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1.1,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsOutOfRange()
                                            .WithCategory(Lumber)
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Machinery",
                    Description = "{score} the score when the played building is {lumber}\n" +
                                  "Gains {multchange} when there is an {iron} building in range",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition(Lumber),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1.5,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .WithCategory(Iron)
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Serving",
                    Description = "{score} score when the played \nbuilding is a {lumber} building",
                    BuyPrice = 3,
                    EventAction = new ScoringAction
                    {
                        Addition = 70,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>()
                            .And(new BuildingCategoryCondition(Lumber)),
                    },
                },
                new()
                {
                    Name = "Resolve",
                    Description = "Retrigger the {city} building\nif there is a {statue} in range",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        RemainingTriggers = 1,
                        Condition = new GameEventCondition<RetriggerStepEvent>()
                            .And(new BuildingCategoryCondition(City))
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(Statue)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }),
                    }.And(new ResetRetriggersAction()),
                },
                new()
                {
                    Name = "Madness",
                    Description = "Retrigger the {lumber} building\nif there is an {iron} building in range",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        RemainingTriggers = 1,
                        Condition = new GameEventCondition<RetriggerStepEvent>()
                            .And(new BuildingCategoryCondition(Lumber))
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(Iron)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }),
                    }.And(new ResetRetriggersAction()),
                },
                new()
                {
                    Name = "Loner",
                    Description = "{score} the score.\n" +
                                  "Gains extra {multchange} when there is\n" +
                                  "a city building out of range",
                    BuyPrice = 8,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.1,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    }.And(new ScoreScalingAction
                    {
                        MultiplierChange = 0.05,
                        Condition = new GameEventCondition<BuildingPlacedEvent>()
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsOutOfRange()
                                    .WithCategory(City)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }),
                    }),
                },
                new()
                {
                    Name = "Museum",
                    Description = "{score} the score\n" +
                                  "when at least 2 {statue} buildings\n" +
                                  " are in range",
                    BuyPrice = 6,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                        {
                            Source = new BuildingFromCurrentEvent()
                                .GetBuildingsInRange()
                                .WithCategory(Statue)
                                .Count(),
                            Value = 1,
                            ComparisonMode = CountCondition.Mode.More,
                        }),
                    },
                },
                new()
                {
                    Name = "Hater",
                    Description = "{score} when a {small} building <e>is triggered</e>.\n\n" +
                                  "Gains extra {addchange} when there is a {medium} building in range",
                    BuyPrice = 3,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingSizeCondition(Small),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 10,
                                Condition = new GameEventCondition<BuildingTriggeredEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 2,
                                Condition = new GameEventCondition<BuildingPlacedEvent>().And(new CountCondition
                                {
                                    Source = new BuildingFromCurrentEvent()
                                        .GetBuildingsInRange()
                                        .WithSize(Medium)
                                        .Count(),
                                    Value = 0,
                                    ComparisonMode = CountCondition.Mode.More,
                                }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Smol",
                    Description = "{score} score when the played building is {small}.\n\n" +
                                  "{probability} to gain extra {addchange}\n" +
                                  "when the {small} building <e>is placed",
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingSizeCondition(Small),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 20,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 10,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new ProbabilityCondition(1, 4)),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Buddies",
                    Description = "{score} the score\n" +
                                  "when a {lumber} and {iron} buildings are in range\n" +
                                  "Gains extra {multchange} when the building placed is {medium}",
                    BuyPrice = 6,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Condition = new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(Lumber)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(Iron)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 2,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new BuildingSizeCondition(Medium)),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Piggy Bank",
                    Description = "Pays {money} at the end of the round\n" +
                                  "Loses <d>$1</d> when a building <e>is placed</e>.\n" +
                                  "Reset when the round starts",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ChangeMoneyAction
                            {
                                Change = 10,
                                IsImmediate = false,
                                Condition = new GameEventCondition<RoundEndEvent>(),
                            },
                            new ScaleChangeMoneyAction
                            {
                                Change = -1,
                                Condition = new GameEventCondition<BuildingPlacedEvent>(),
                            },
                            new ScaleChangeMoneyAction
                            {
                                Set = 10,
                                Condition = new GameEventCondition<RoundStartEvent>(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Warm",
                    Description = "When the round ends,\n earn {money} for every \n{farming} building in your hand",
                    BuyPrice = 3,
                    EventAction = new ChangeMoneyAction
                    {
                        Change = 1,
                        Multiplier = new BuildingsInHand()
                            .WithCategory(Farming)
                            .Count(),
                        IsImmediate = false,
                        Condition = new GameEventCondition<RoundEndEvent>(),
                    },
                },
                new()
                {
                    Name = "Black Market",
                    Description = "Duplicate items can show up in the shop",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                },
                // new()
                // {
                //     Name = "Gift Card",
                //     Description = "When round ends,\n Adds <d>$1</d> to the \nsell value of owned boosters",
                //     BuyPrice = 5,
                // },
                // new()
                // {
                //     Name = "To Do",
                //     Description = "Earn {money} if the building is {building}\nBuilding changes every round",
                //     BuyPrice = 3,
                // },
                new()
                {
                    Name = "Dog",
                    Description = "When the round starts,\n Retrigger the first building you place 2 more times",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 2,
                        Condition = new GameEventCondition<RetriggerStepEvent>().And(new CountCondition
                        {
                            Source = new TotalBuildingsPlayedThisRound(),
                            Value = 0,
                            ComparisonMode = CountCondition.Mode.Equal,
                        }),
                    }.And(new ResetRetriggersAction()),
                },
                new()
                {
                    Name = "Heavy",
                    Description = "{score} score when the played\n" +
                                  "building is {iron}.\n\n" +
                                  "Gains extra {addchange} when there\n" +
                                  "is an {iron} building in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition(Iron),
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 30,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 8,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .WithCategory(Iron)
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "ROBOX",
                    Description = "When a {farming} building <e>is triggered</e>,\n" +
                                  "{score} score for every {farming}\n" +
                                  "building played in this run.",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new MultipliedScoringAction
                    {
                        Addition = 5,
                        Factor = new CategoryPlayCountSource { Category = Farming },
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    },
                },
                new()
                {
                    Name = "Wood Working",
                    Description = "{score} when the played building is {lumber}.\n\n" +
                                  "Gains extra {addchange} when a {statue} building\n" +
                                  "is in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Condition = new BuildingCategoryCondition
                        {
                            Categories = new[] { Lumber },
                            Source = new BuildingFromCurrentEvent(),
                        },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 30,
                                Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 8,
                                Condition = new GameEventCondition<BuildingPlacedEvent>()
                                    .And(new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .WithCategory(Statue)
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    }),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Murder",
                    Description = "{score} per {red} card in hand",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new MultipliedScoringAction
                    {
                        Addition = 30,
                        Factor = new BuildingsInHand()
                            .WithColor(Red)
                            .Count(),
                        Condition = new GameEventCondition<FinalScoreReadyEvent>(),
                    },
                },
                new()
                {
                    Name = "Duo",
                    Description = "{score} the score when there is \n" +
                                  "a {statue} and {city} building in range",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Condition = new GameEventCondition<FinalScoreReadyEvent>()
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(Statue)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            })
                            .And(new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsInRange()
                                    .WithCategory(City)
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            }),
                    },
                },
                // new()
                // {
                //     Name = "Plot Twist",
                //     Description =
                //         "When a {farming} building\nhas 3 or more {farming} buildings \nof the same color in range,\n{score} the score",
                //     BuyPrice = 6,
                //     Rarity = Rare,
                //     EventAction = new ScoringAction
                //     {
                //         Multiplier = 5,
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<FinalScoreReadyEvent>(),
                //             new BuildingCategoryCondition
                //             {
                //                 Categories = new[] { Farming },
                //                 Source = new BuildingFromCurrentEvent(),
                //             },
                //             // new CountCondition
                //             // {
                //             //     Source = new BuildingFromCurrentEvent()
                //             //         .GetBuildingsInRange()
                //             //         .WithCondition(new BuildingCategoryCondition { Categories = new[] { Farming } })
                //             //         .GetColors()
                //             //         .Count(),
                //             //     Value = 2,
                //             //     ComparisonMode = CountCondition.Mode.More,
                //             // },
                //         },
                //     },
                // },
            };
        }

        private static BoosterCard CreateColorBooster(string name, ColorTag color) =>
            new()
            {
                Name = name,
                Description = $"{{add}} score when a {{{color}}}\n" +
                              "building <e>is triggered</e>.\n\n" +
                              "Gains extra {addchange} for \n" +
                              $"every 2 {{{color}}} building played.",
                BuyPrice = 3,
                EventAction = new ScoringAction
                {
                    Addition = 20,
                    Condition = new GameEventCondition<BuildingTriggeredEvent>()
                        .And(new ColorCheckCondition(color)),
                }.And(new ScoreScalingAction
                {
                    AdditionChange = 10,
                    Delay = 2,
                    OneTime = false,
                    Condition = new GameEventCondition<BuildingPlacedEvent>()
                        .And(new ColorCheckCondition(color)),
                }),
            };

        private static BoosterCard CreateCategoryBooster(string name, Category category) =>
            new()
            {
                Name = name,
                Description = $"{{add}} score when \n{{{category}}} building <e>is placed</e>",
                BuyPrice = 3,
                EventAction = new ScoringAction
                {
                    Addition = 90,
                    Condition = new GameEventCondition<BuildingPlacedEvent>()
                        .And(new BuildingCategoryCondition(category)),
                },
            };
    }
}