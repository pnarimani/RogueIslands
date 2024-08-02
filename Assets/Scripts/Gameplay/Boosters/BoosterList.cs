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
                        Conditions = new[] { GameEventCondition.Create<FinalScoreReadyEvent>() },
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
                        Conditions = new[] { GameEventCondition.Create<BuildingAddedEvent>() },
                        RangeMultiplier = 2,
                        Source = new BuildingFromCurrentEvent(),
                    },
                },
                new()
                {
                    Name = "Sweatshop",
                    Description = "Extra {mult} for each {red} building in range",
                    Rarity = Uncommon,
                    BuyPrice = 5,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new[] { GameEventCondition.Create<FinalScoreReadyEvent>() },
                        Factor = new BuildingFromCurrentEvent()
                            .GetBuildingsInRange()
                            .With(new ColorCheckCondition { ForcedColors = new[] { Red } })
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
                    Description = "{probability} to {mult} the score\n when a {lumber} building <e>is triggered",
                    BuyPrice = 6,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggeredEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Lumber },
                            },
                            new ProbabilityCondition
                            {
                                FavorableOutcome = 1,
                                TotalOutcomes = 2,
                            },
                        },
                        Multiplier = 2.5,
                    },
                },
                new()
                {
                    Name = "Maizinator",
                    Description = "{farming} buildings <e>trigger</e> one more time",
                    BuyPrice = 7,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<RetriggerStepEvent>().Or<BuildingPlacedEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Farming },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Rotten Egg",
                    Description = "Gains <d>$2</d> sell value \nat the end of the round",
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new GainSellValueAction
                            {
                                Amount = 2,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEndEvent>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Campfire",
                    Description = "Gains {multchange} for each booster sold.\n Resets at the end of the act",
                    BuyPrice = 6,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BoosterSoldEvent>(),
                                },
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<ActEndEvent>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "RealState Agent",
                    Description = "For every 8 buildings placed,\n gains {multchange} mult",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.2,
                                Delay = 8,
                                OneTime = false,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Digger",
                    Description = "Pays {money} at the end of the round",
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction
                    {
                        Conditions = new IGameCondition[] { GameEventCondition.Create<RoundEndEvent>() },
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<RetriggerStepEvent>().Or<BuildingPlacedEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Iron },
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
                        "{mult} the <e>final score</e>\nif at least one building of each color\n is placed down",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
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
                    Description = "{mult} the <e>final score</e>\nif all buildings are {green} or {purple}",
                    BuyPrice = 8,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new ColorCheckCondition
                            {
                                BannedColors = new[] { Red, Blue },
                                Source = new PlacedDownBuildings(),
                            },
                        },
                        Multiplier = 5,
                    },
                },
                new()
                {
                    Name = "Gennaro",
                    Description = "...",
                    BuyPrice = 3,
                    EventAction = new RandomScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                        Multiplier = 5,
                    },
                },
                new()
                {
                    Name = "The Rat",
                    Description = "On the start of the round,\ndestroys a random booster.\nGains <m>x1</m> multiplier.",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new RatAttack
                            {
                                Change = 1,
                                Conditions = new[] { GameEventCondition.Create<RoundStartEvent>() },
                            },
                            new ScoringAction
                            {
                                Conditions = new[] { GameEventCondition.Create<FinalScoreReadyEvent>() },
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
                        Conditions = new[] { GameEventCondition.Create<FinalScoreReadyEvent>() },
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
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new DestroyBoosterAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEndEvent>(),
                                    new ProbabilityCondition
                                    {
                                        FavorableOutcome = 1,
                                        TotalOutcomes = 6,
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
                    Description = "{score} when a {city} building <e>is triggered",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggeredEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { City },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Late Bloomer",
                    Description = "Starts at <m>X0.5</m> score.\n After 5 rounds, it turns into <m>X2</m> mult",
                    BuyPrice = 3,
                    SellPrice = 2,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Conditions = new[] { GameEventCondition.Create<FinalScoreReadyEvent>() },
                                Multiplier = 0.5,
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 1.5,
                                Delay = 5,
                                OneTime = true,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEndEvent>(),
                                },
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Capitalist",
                    Description = "{score} for every <d>$2</d> you have\nwhen a building <e>is placed",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                        },
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggeredEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Iron },
                            },
                        },
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
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = -0.25,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEndEvent>(),
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
                    Description = "Gains {multchange} every time you <e>reroll</e> in the shop",
                    BuyPrice = 8,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 0,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.05,
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
                    Description = "Copies the booster to the right",
                    BuyPrice = 10,
                    Rarity = Rare,
                    EventAction = new CopyBoosterAction(),
                },
                new()
                {
                    Name = "Hug",
                    Description = "{mult} for each {Large} building in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                        Factor = new BuildingsByRange
                            {
                                Center = new BuildingFromCurrentEvent(),
                                ReturnInRange = true,
                            }.With(new BuildingSizeCondition { Allowed = new[] { Large } })
                            .Count(),
                        Multiplier = 0.25,
                    },
                },
                new()
                {
                    Name = "Sizing",
                    Description = "{score} if there is at least one {small},\n {medium} and {large} building in range",
                    BuyPrice = 6,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(
                                        new BuildingSizeCondition { Allowed = new[] { Small } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingSizeCondition
                                        { Allowed = new[] { Medium } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(
                                        new BuildingSizeCondition { Allowed = new[] { Large } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                        Multiplier = 1.5,
                    },
                },
                new()
                {
                    Name = "Away",
                    Description = "{score} for each {small} building out of range",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                        Factor = new BuildingFromCurrentEvent()
                            .GetBuildingsOutOfRange()
                            .With(new BuildingSizeCondition { Allowed = new[] { Small } })
                            .Count(),
                        Multiplier = .1,
                    },
                },
                new()
                {
                    Name = "Quiet",
                    Description = "{score} when the played building is {city}\n" +
                                  "Gains extra {addchange} when a {lumber}\n" +
                                  " building is out of range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 70,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { City },
                                        Source = new BuildingFromCurrentEvent(),
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 10,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsOutOfRange()
                                            .With(new BuildingCategoryCondition
                                                { Categories = new[] { Lumber } })
                                            .Count(),
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Industry",
                    Description = "When the placed {Iron} building\n has {Lumber} buildings in range\n{score} score",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Iron },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition
                                        { Categories = new[] { Lumber } })
                                    .Count(),
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
                        "When the placed {Statue} building\n has no other {Statue} building in range\n{score} score",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Statue },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition
                                        { Categories = new[] { Statue } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.Equal,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Colorful",
                    Description = "{mult} <e>color bonus",
                    BuyPrice = 3,
                    Rarity = Uncommon,
                    EventAction = new BonusAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonusEvent>(),
                        },
                        ColorMultiplier = 2,
                    },
                },
                new()
                {
                    Name = "Good Year",
                    Description = "{score} <e>bonus",
                    BuyPrice = 3,
                    Rarity = Uncommon,
                    EventAction = new BonusAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonusEvent>(),
                        },
                        Addition = 15,
                    },
                },
                new()
                {
                    Name = "Tourism",
                    Description = "{score} <e>bonus</e> score from {Statue} buildings",
                    BuyPrice = 4,
                    Rarity = Rare,
                    EventAction = new BonusAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonusEvent>(),
                            new BuildingCategoryCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Categories = new[] { Statue },
                            },
                        },
                        Multiplier = 1.5,
                    },
                },
                new()
                {
                    Name = "Inside Out",
                    Description = "Instead of in-range buildings,\n out of range buildings score <e>bonus</e>",
                    BuyPrice = 8,
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterBuildingBonusEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Mine Shaft",
                    Description = "{score} score when an {Iron} building <e>is triggered</e>\n" +
                                  "Gains extra {addchange} when a {Lumber} building is in of range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 40,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingTriggeredEvent>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { Iron },
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .With(new BuildingCategoryCondition
                                                { Categories = new[] { Lumber } })
                                            .Count(),
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "The Wall",
                    Description =
                        "Gains <m>X0.3</m> mult \nwhen a {medium} or {large} building <e>is placed</e>.\nResets when a {small} building <e>is placed",
                    BuyPrice = 6,
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.3,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new BuildingSizeCondition
                                    {
                                        Allowed = new[] { Medium, Large },
                                    },
                                },
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new BuildingSizeCondition
                                    {
                                        Allowed = new[] { Small },
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Black Coat",
                    Description = "{score} score when you have <d>$6</d> or less",
                    Rarity = Rare,
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new CountCondition
                            {
                                Source = new MoneyAmount(),
                                Value = 7,
                                ComparisonMode = CountCondition.Mode.Less,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Decorations",
                    Description = "{score} score when the played building is {statue}\n" +
                                  "Gains extra {addchange} when a {city} building is in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Statue },
                                Source = new BuildingFromCurrentEvent(),
                            },  
                        },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 50,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 8,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .With(new BuildingCategoryCondition { Categories = new[] { City } })
                                            .Count(),
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "More Interest",
                    Description = "Earn <d>$1</d> more in interest\n per <d>$5</d> in the bank",
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "Food",
                    Description =
                        "{score} when a {large} is played.\nGains extra {addchange} the building is also {farming}",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Large },
                            },
                        },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 30,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 15,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { Farming },
                                        Source = new BuildingFromCurrentEvent(),
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Free Reroll",
                    Description = "Get a free <e>reroll</e> in every shop",
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "Last Breath",
                    Description = "After you played your last building\nRetrigger all buildings",
                    BuyPrice = 5,
                    Rarity = Rare,
                },
                new()
                {
                    Name = "Blue Jeans",
                    Description =
                        "{score} <e>bonus</e> from {blue} buildings\nGains <a>+1</a> when a {blue} building <e>is placed\nLoses <a>1</a> when a {red} building <e>is placed",
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new List<GameAction>
                        {
                            new BonusAction
                            {
                                Addition = 10,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingBonusEvent>(),
                                    new ColorCheckCondition
                                    {
                                        Source = new BuildingFromCurrentEvent(),
                                        ForcedColors = new[] { Blue },
                                    },
                                },
                            },
                            new BonusScalingAction
                            {
                                AdditionChange = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new ColorCheckCondition
                                    {
                                        Source = new BuildingFromCurrentEvent(),
                                        ForcedColors = new[] { Blue },
                                    },
                                },
                            },
                            new BonusScalingAction
                            {
                                AdditionChange = -1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new ColorCheckCondition
                                    {
                                        Source = new BuildingFromCurrentEvent(),
                                        ForcedColors = new[] { Red },
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Modular",
                    Description = "{score} score when a {small} building <e>is placed",
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Addition = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Small },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Restraint",
                    Description =
                        "Every round, a card is\n selected from your hand.\n If you don't play that card,\n earn <d>$7</d> at the end of the round",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                },
                new()
                {
                    Name = "Mid",
                    Description = "{score} score when a {medium} building <e>is placed",
                    BuyPrice = 2,
                    EventAction = new ScoringAction
                    {
                        Addition = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Medium },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Subsidized",
                    Description =
                        "Prevents loss\n if your score is at least \n<a>%25</a> of the required score.\nSelf destructs when activated.",
                    BuyPrice = 4,
                },
                new()
                {
                    Name = "Extra Time",
                    Description = "Retrigger the next 8 buildings",
                    BuyPrice = 6,
                    Rarity = Uncommon,
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Erosion",
                    Description =
                        "Starts at <m>X2</m> score on every round.\nLoses <m>X0.05</m> score per building played",
                    BuyPrice = 7,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 2,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = -0.05,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 2,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundStartEvent>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Freebie",
                    Description = "Buildings in the shop are <d>$1</d> cheaper",
                    BuyPrice = 3,
                },
                new()
                {
                    Name = "Hold Off",
                    Description = "Extra <m>X0.1</m> score \nfor every {statue} building in hand",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                        Factor = new BuildingsInHand()
                            .With(new BuildingCategoryCondition { Categories = new[] { Statue } })
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Large },
                            },
                        },
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
                    Rarity = Rare,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 20,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingTriggeredEvent>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { City },
                                        Source = new BuildingFromCurrentEvent(),
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<AfterBuildingBonusEvent>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { City },
                                        Source = new BuildingFromCurrentEvent(),
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Bronze Lion",
                    Description = "When the played building is {statue}\n" +
                                  "{score} to score\n" +
                                  "Gains extra {addchange} when an\n" +
                                  "{iron} building in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Statue },
                                Source = new BuildingFromCurrentEvent(),
                            },
                        },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 40,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 8,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .With(new BuildingCategoryCondition { Categories = new[] { Iron } })
                                            .Count(),
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Perimeter",
                    Description = "{score} score when a {lumber} building <e>is triggered</e>\n" +
                                  "Gains extra {addchange} when there is \n" +
                                  "a {lumber} building out of range",
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 70,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingTriggeredEvent>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { Lumber },
                                        Source = new BuildingFromCurrentEvent(),
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 10,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        ComparisonMode = CountCondition.Mode.More,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsOutOfRange()
                                            .With(new BuildingCategoryCondition
                                                { Categories = new[] { Lumber } })
                                            .Count(),
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Plot",
                    Description = "{score} score when a {farming} building <e>is triggered</e>\n" +
                                  "Gains extra {addchange} when another {farming}\n" +
                                  "building is in range",
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 40,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingTriggeredEvent>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { Farming },
                                        Source = new BuildingFromCurrentEvent(),
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .With(new BuildingCategoryCondition
                                                { Categories = new[] { Farming } })
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Tetros",
                    Description =
                        "{score} score when a {medium} building <e>is placed</e>\nGains extra {addchange} when a {large} building is in range",
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 25,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new BuildingSizeCondition
                                    {
                                        Source = new BuildingFromCurrentEvent(),
                                        Allowed = new[] { Medium },
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        Source = new BuildingsByRange
                                            {
                                                Center = new BuildingFromCurrentEvent(),
                                                ReturnInRange = true,
                                            }.With(new BuildingSizeCondition
                                                { Allowed = new[] { Large } })
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    },
                                },
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new CountCondition
                            {
                                Source = new BuildingFromCurrentEvent()
                                    .GetBuildingsOutOfRange()
                                    .With(new ColorCheckCondition { ForcedColors = new[] { Blue } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
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
                        Multiplier = new BuildingFromCurrentEvent()
                            .GetBuildingsInRange()
                            .With(new ColorCheckCondition { ForcedColors = new[] { Green } })
                            .Count(),
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Purple Heart",
                    Description = "{score} score when a building <e>is placed</e>\n" +
                                  "Gains extra {addchange} when there is\n" +
                                  "a {purple} building out of range",
                    BuyPrice = 3,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 50,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 2,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsOutOfRange()
                                            .With(new ColorCheckCondition { ForcedColors = new[] { Purple } })
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Si",
                    Description = "When a {small} building scores <e>bonus</e>\n {score} score",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterBuildingBonusEvent>(),
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Small },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Edge",
                    Description =
                        "Gains <m>X0.5</m> mult\nfor every {large} building placed.\nResets at the end of the round",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                MultiplierChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new BuildingSizeCondition
                                    {
                                        Allowed = new[] { Large },
                                    },
                                },
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundEndEvent>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Overtime",
                    Description = "When the played building is {large}\n {score} score",
                    BuyPrice = 8,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Large },
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggeredEvent>(),
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Small },
                            },
                            new ProbabilityCondition
                            {
                                FavorableOutcome = 1,
                                TotalOutcomes = 4,
                            },
                        },
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggeredEvent>(),
                            new BuildingSizeCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Allowed = new[] { Medium },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Debut",
                    Description = "Every round, the first building\n" +
                                  "of each size scores {score} <e>when placed</e>\n" +
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
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition()
                                    {
                                        ComparisonMode = CountCondition.Mode.Equal,
                                        Value = 0,
                                        Source = new BuildingFromCurrentEvent()
                                            .GetSizes()
                                            .GetPlayCountThisRound(),
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 50,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<ActEndEvent>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "No Smith",
                    Description = "When a {city} building\nhas no {iron} building in range,\n{score} score",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { City },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Iron } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.Equal,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Watch",
                    Description = "When a {farming} building \nhas a {lumber} building in range,\n {score} the score",
                    BuyPrice = 4,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Farming },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Lumber } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Machinery",
                    Description = "When a {city} building has \n{iron} building in range,\n {score} the score",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { City },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Iron } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
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
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Lumber },
                                Source = new BuildingFromCurrentEvent(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Resolve",
                    Description = "Retrigger the {city} building\nif there is a {statue} in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        RemainingTriggers = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggeredEvent>().Or<BuildingPlacedEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { City },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Statue } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Madness",
                    Description = "Retrigger the {lumber} building\nif there is an {iron} building in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        RemainingTriggers = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<RetriggerStepEvent>().Or<BuildingPlacedEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Lumber },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Iron } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Loner",
                    Description = "When a {city} building\nhas no other {city} building in range\n{score} the score",
                    BuyPrice = 6,
                    Rarity = Uncommon,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { City },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { City } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.Equal,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Museum",
                    Description = "When the {statue} building\nhas a {statue} building in range,\n{score} the score",
                    BuyPrice = 6,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 1.5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlacedEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Statue },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Statue } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Hater",
                    Description = "{score} when a {small} building <e>is triggered</e>\n" +
                                  "Gains extra {addchange} when there is a {medium} building in range",
                    BuyPrice = 3,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 25,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingTriggeredEvent>(),
                                    new BuildingSizeCondition
                                    {
                                        Source = new BuildingFromCurrentEvent(),
                                        Allowed = new[] { Small },
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 2,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        Source = new BuildingsByRange
                                            {
                                                Center = new BuildingFromCurrentEvent(),
                                                ReturnInRange = true,
                                            }.With(new BuildingSizeCondition
                                                { Allowed = new[] { Medium } })
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Smol",
                    Description = "{score} score when the played building is {small}\n" +
                                  "{probability} to gain extra {addchange}\n" +
                                  "when a {small} building <e>is placed",
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 40,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                    new BuildingSizeCondition
                                    {
                                        Source = new BuildingFromCurrentEvent(),
                                        Allowed = new[] { Small },
                                    },
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 10,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new BuildingSizeCondition
                                    {
                                        Source = new BuildingFromCurrentEvent(),
                                        Allowed = new[] { Small },
                                    },
                                    new ProbabilityCondition
                                    {
                                        FavorableOutcome = 1,
                                        TotalOutcomes = 4,
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Buddies",
                    Description = "When a farming building <e>is triggered, \n" +
                                  "if there is at least 1 {lumber} and \n" +
                                  "1 {iron} building in range\n{score} the score",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggeredEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Farming },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Lumber } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Iron } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Piggy Bank",
                    Description =
                        "On round start, it holds <d>$10</d>.\nLoses <d>$1</d> when a building <e>is placed</e>.\nCash out when round ends",
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ChangeMoneyAction
                            {
                                Change = 10,
                                IsImmediate = false,
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
                            .With(new BuildingCategoryCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Categories = new[] { Farming },
                            }).Count(),
                        IsImmediate = false,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<RoundEndEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Black Market",
                    Description = "Duplicate items can show up in the shop",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                },
                new()
                {
                    Name = "Gift Card",
                    Description = "When round ends,\n Adds <d>$1</d> to the \nsell value of owned boosters",
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "To Do",
                    Description = "Earn {money} if the building is {building}\nBuilding changes every round",
                    BuyPrice = 3,
                },
                new()
                {
                    Name = "Dog",
                    Description = "When the round starts,\n Retrigger the first building you place 2 more times",
                    BuyPrice = 5,
                    Rarity = Uncommon,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<RetriggerStepEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Heavy",
                    Description = "{score} score when the played\n" +
                                  "building is {iron}\n" +
                                  "Gains extra {addchange} when there\n" +
                                  "is an {iron} building in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Iron },
                                Source = new BuildingFromCurrentEvent(),
                            },
                        },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 70,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 8,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .With(
                                                new BuildingCategoryCondition { Categories = new[] { Iron } })
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "ROBOX",
                    Description = "When a {farming} building <e>is triggered</e>\n" +
                                  "Score <a>+5</a> for every {farming}\n" +
                                  "building played this run",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new MultipliedScoringAction
                    {
                        Addition = 5,
                        Factor = new CategoryPlayCountSource { Category = Farming },
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Wood Working",
                    Description = "{score} when the played building is {lumber}\n" +
                                  "Gains extra {addchange} when a {statue} building\n" +
                                  "is in range",
                    BuyPrice = 4,
                    Rarity = Uncommon,
                    EventAction = new CompositeAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Lumber },
                                Source = new BuildingFromCurrentEvent(),
                            },
                        },
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Addition = 70,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<FinalScoreReadyEvent>(),
                                },
                            },
                            new ScoreScalingAction
                            {
                                AdditionChange = 8,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlacedEvent>(),
                                    new CountCondition
                                    {
                                        Source = new BuildingFromCurrentEvent()
                                            .GetBuildingsInRange()
                                            .With(new BuildingCategoryCondition { Categories = new[] { Statue } })
                                            .Count(),
                                        Value = 0,
                                        ComparisonMode = CountCondition.Mode.More,
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Murder",
                    Description = "{score} per {red} card in hand",
                    BuyPrice = 6,
                    Rarity = Uncommon,
                    EventAction = new MultipliedScoringAction
                    {
                        Addition = 20,
                        Factor = new BuildingsInHand()
                            .With(new ColorCheckCondition { ForcedColors = new[] { Red } })
                            .Count(),
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                        },
                    },
                },
                new()
                {
                    Name = "Probe",
                    Description = "{probability} to {score} <e>bonus",
                    BuyPrice = 4,
                    EventAction = new BonusAction
                    {
                        Multiplier = 5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonusEvent>(),
                            new ProbabilityCondition
                            {
                                FavorableOutcome = 1,
                                TotalOutcomes = 3,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Duo",
                    Description =
                        "When an {iron} building has both\n {statue} and {city} buildings in range,\n {score} the score",
                    BuyPrice = 5,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 3,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Iron },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.With(new BuildingCategoryCondition { Categories = new[] { Statue } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                {
                                    Center = new BuildingFromCurrentEvent(),
                                    ReturnInRange = true,
                                }.With(new BuildingCategoryCondition { Categories = new[] { City } }).Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.More,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Plot Twist",
                    Description =
                        "When a {farming} building\nhas 3 or more {farming} buildings \nof the same color in range,\n{score} the score",
                    BuyPrice = 6,
                    Rarity = Rare,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<FinalScoreReadyEvent>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Farming },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            // new CountCondition
                            // {
                            //     Source = new BuildingFromCurrentEvent()
                            //         .GetBuildingsInRange()
                            //         .WithCondition(new BuildingCategoryCondition { Categories = new[] { Farming } })
                            //         .GetColors()
                            //         .Count(),
                            //     Value = 2,
                            //     ComparisonMode = CountCondition.Mode.More,
                            // },
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
                Description =
                    $"{{add}} score when \na {{{color}}} building <e>is triggered</e>\nGains extra {{addchange}} for every 5 {{{color}}} building played.",
                BuyPrice = 3,
                EventAction = new CompositeAction
                {
                    Actions = new GameAction[]
                    {
                        new ScoringAction
                        {
                            Addition = 40,
                            Conditions = new IGameCondition[]
                            {
                                GameEventCondition.Create<BuildingTriggeredEvent>(),
                                new ColorCheckCondition
                                {
                                    Source = new BuildingFromCurrentEvent(),
                                    ForcedColors = new List<ColorTag> { color },
                                },
                            },
                        },
                        new ScoreScalingAction
                        {
                            AdditionChange = 5,
                            Delay = 5,
                            OneTime = false,
                            Conditions = new IGameCondition[]
                            {
                                GameEventCondition.Create<BuildingPlacedEvent>(),
                                new ColorCheckCondition
                                {
                                    Source = new BuildingFromCurrentEvent(),
                                    ForcedColors = new List<ColorTag> { color },
                                },
                            },
                        },
                    },
                },
            };
        }

        private static BoosterCard CreateCategoryBooster(string name, Category category)
        {
            return new BoosterCard
            {
                Name = name,
                Description = $"{{add}} score when \n{{{category}}} building <e>is placed</e>",
                BuyPrice = 3,
                EventAction = new CompositeAction
                {
                    Actions = new GameAction[]
                    {
                        new ScoringAction
                        {
                            Addition = 90,
                            Conditions = new IGameCondition[]
                            {
                                GameEventCondition.Create<BuildingPlacedEvent>(),
                                new BuildingCategoryCondition
                                {
                                    Source = new BuildingFromCurrentEvent(),
                                    Categories = new List<Category> { category },
                                },
                            },
                        },
                    },
                },
            };
        }
    }
}