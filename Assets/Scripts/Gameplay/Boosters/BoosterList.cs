﻿using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Descriptions;
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
                    Description = "{score} score",
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
                    Description = "All bonus scores count\n as normal triggers",
                    BuyPrice = 10,
                },
                CreateColorBooster("Purple Boost", ColorTag.Purple),
                CreateColorBooster("Green Boost", ColorTag.Green),
                CreateColorBooster("Red Boost", ColorTag.Red),
                CreateColorBooster("Blue Boost", ColorTag.Blue),
                new()
                {
                    Name = "Network",
                    Description = "+100% range for all buildings",
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
                    Description = "{mult} for each {red} building in range",
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
                    Description = "{Red} and {Blue} count as the same.\n{Green} and {Purple} count as the same",
                    BuyPrice = 5,
                },
                new()
                {
                    Name = "Rigged",
                    Description = "Double all probabilities",
                    BuyPrice = 4,
                },
                new()
                {
                    Name = "Saw Dust",
                    Description = "{probability} to give {mult} for each {lumber} building triggered",
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Lumber },
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
                    Description = "{lumber} buildings trigger one more time",
                    BuyPrice = 7,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterBuildingScoreTrigger>().Or<ResetRetriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Farming },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Rotten Egg",
                    Description = "-10% score\nGains $5 sell value at the end of the round",
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
                    Description = "Gains {multchange} for each booster sold.\n Resets at the end of the act",
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
                new()
                {
                    Name = "RealState Agent",
                    Description = "For every 8 buildings placed, gains {multchange} mult",
                    BuyPrice = 5,
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
                                Delay = 8,
                                OneTime = false,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlaced>(),
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
                        Conditions = new IGameCondition[] { GameEventCondition.Create<RoundEnd>() },
                        Change = 4,
                    },
                },
                new()
                {
                    Name = "Ukulele",
                    Description = "{probability} to trigger {iron} buildings 2 more times",
                    BuyPrice = 6,
                    EventAction = new RetriggerBuildingAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterBuildingScoreTrigger>().Or<ResetRetriggers>(),
                            new BuildingCategoryCondition()
                            {
                                Categories = new[] { Category.Iron },
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
                    Description = "{mult} if at least \none building of each color\n is placed down",
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
                    Description = "{mult} if all buildings are {green} or {purple}",
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
                    Description = "...",
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
                    Description = "{mult} if 6 or more buildings are placed down",
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new CountCondition
                            {
                                Source = new PlacedDownBuildings().Count(),
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
                    Description = "On the start of the round,\ndestroys a random booster.\nGains x4 multiplier.",
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
                    Description = ".25x score for each different\n building placed in the world",
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
                    Description = "{mult} score, {probability} to get destroyed at the end of the round",
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
                    Description = "{mult} when a {city} building is triggered",
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.City },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Country Carl",
                    Description = "{add} when {farming} building is triggered",
                    BuyPrice = 4,
                    EventAction = new ScoringAction
                    {
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Farming },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Ice Cream",
                    Description = "Starts with {add}\nand {addchange} score after each building is placed",
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
                    Description = "Starts at 0.5x score.\n After 5 rounds, it turns into x100 mult",
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
                    Description = "{add} when a building is placed down",
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Products = 25,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlaced>(),
                        },
                    },
                },
                new()
                {
                    Name = "Capitalist",
                    Description = "{score} for every $2 you have\n when a building is placed down",
                    BuyPrice = 7,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingPlaced>(),
                        },
                        Products = 5,
                        Factor = new MoneyAmount { DivideBy = 2 },
                    },
                },
                new()
                {
                    Name = "Gems",
                    Description = "{iron} buildings earn {money} when triggered",
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
                                Categories = new[] { Category.Iron },
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
                    Description = "Gains 1x mult every time you reroll in the shop",
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
                    Description = "Copies the booster to the right",
                    BuyPrice = 10,
                    EventAction = new CopyBoosterAction(),
                },
                new()
                {
                    Name = "Hug",
                    Description = "{mult} for each {Large} building in range",
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
                        Multiplier = 2,
                    },
                },
                new()
                {
                    Name = "Sizing",
                    Description = "{score} if there is at least one {small},\n {medium} and {large} building in range",
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
                                    }.WithCondition(
                                        new BuildingSizeCondition { Allowed = new[] { BuildingSize.Small } })
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
                                    }.WithCondition(new BuildingSizeCondition
                                        { Allowed = new[] { BuildingSize.Medium } })
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
                                    }.WithCondition(
                                        new BuildingSizeCondition { Allowed = new[] { BuildingSize.Large } })
                                    .Count(),
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
                    Description = "+0.3x for each small building out of range",
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
                    Description = "{score} when a {City} building has no {Lumber} building in range",
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 10,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.City },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.WithCondition(new BuildingCategoryCondition
                                        { Categories = new[] { Category.Lumber } })
                                    .Count(),
                                Value = 0,
                                ComparisonMode = CountCondition.Mode.Equal,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Industry",
                    Description = "{score} when an {Iron} building has {Lumber} buildings in range",
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 5,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Iron },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.WithCondition(new BuildingCategoryCondition
                                        { Categories = new[] { Category.Lumber } })
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
                    Description = "{score} score\n when {Statue} building is placed down\n without any other {Statue} buildings in range",
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 3,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Statue },
                                Source = new BuildingFromCurrentEvent(),
                            },
                            new CountCondition
                            {
                                Source = new BuildingsByRange
                                    {
                                        Center = new BuildingFromCurrentEvent(),
                                        ReturnInRange = true,
                                    }.WithCondition(new BuildingCategoryCondition
                                        { Categories = new[] { Category.Statue } })
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
                    Description = "{mult} color bonus",
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
                    Description = "{score} bonus",
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
                    Description = "{score} bonus score from {Statue} buildings",
                    BuyPrice = 4,
                    EventAction = new ModifyBonusAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingBonus>(),
                            new BuildingCategoryCondition
                            {
                                Source = new BuildingFromCurrentEvent(),
                                Categories = new[] { Category.Statue },
                            },
                        },
                        Multiplier = 10,
                    },
                },
                new()
                {
                    Name = "Inside Out",
                    Description = "Instead of in-range buildings,\n out of range buildings score bonus",
                    BuyPrice = 4,
                },
                new()
                {
                    Name = "Double Trouble",
                    Description = "{score} bonus when a building scores bonus",
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
                    Description = "{score} score when an {Iron} building is triggered",
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingTriggered>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Iron },
                                Source = new BuildingFromCurrentEvent(),
                            },
                        },
                    },
                },
                new()
                {
                    Name = "The Wall",
                    Description = "Gains +0.3 mult for each \n{medium} and {large} building placed.\nResets when a {small} building is placed",
                    BuyPrice = 6,
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
                                MultiplierChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlaced>(),
                                    new BuildingSizeCondition
                                    {
                                        Allowed = new[] { BuildingSize.Medium, BuildingSize.Large },
                                    },
                                },
                            },
                            new BoosterResetAction
                            {
                                Multiplier = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingPlaced>(),
                                    new BuildingSizeCondition
                                    {
                                        Allowed = new[] { BuildingSize.Small },
                                    },
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Black Coat",
                    Description = "{score} when you have $4 or less",
                    BuyPrice = 4,
                    EventAction = new ScoringAction
                    {
                        Multiplier = 10,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<AfterAllBuildingTriggers>(),
                            new CountCondition()
                            {
                                Source = new MoneyAmount(),
                                Value = 5,
                                ComparisonMode = CountCondition.Mode.Less,
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
                Description = new DescriptionData()
                {
                    Text = $"{{add}} score when {{{color}}} building is triggered",
                },
                BuyPrice = 3,
                EventAction = new ScoringAction
                {
                    Products = 10,
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