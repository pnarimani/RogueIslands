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
                    Name = "Rigged",
                    Description = new LiteralDescription("Double all probabilities"),
                    BuyPrice = 4,
                    EventAction = new ModifyProbabilitiesAction(),
                },
                new()
                {
                    Name = "Saw Dust",
                    Description =
                        new ProbabilityDescription($"{{0}} to give x2 mult for each {Category.Cat3} building scored"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
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
                        XMult = 2,
                    },
                },
                new()
                {
                    Name = "Mr Producer",
                    Description = new LiteralDescription("Permanently add +2 product to played buildings"),
                    BuyPrice = 4,
                    EventAction = new PermanentBuildingUpgradeAction
                    {
                        ProductUpgrade = 2,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                        },
                    },
                },
                new()
                {
                    Name = "Maizinator",
                    Description = new LiteralDescription($"Retrigger all {Category.Cat2} buildings"),
                    BuyPrice = 7,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            BuildingTriggerCountCondition.FirstTrigger,
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat2 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Bad Eyesight",
                    BuyPrice = 7,
                    Description =
                        new LiteralDescription(
                            $"{ColorTag.Red} and {ColorTag.Blue} are the same. {ColorTag.Green} and {ColorTag.Purple} are the same."),
                    EventAction = new ModifyBuildingColorConditionAction(),
                },
                new()
                {
                    Name = "Base 2",
                    Description =
                        new ScalingBoosterDescription("If the number of buildings is a power of 2, gains +4 products"),
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new BoosterScalingAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayStart>(),
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
                                    GameEventCondition.Create<DayEnd>(),
                                },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Rotten Egg",
                    Description =
                        new LiteralDescription("-20 products, gains 5$ of sell value at the end of the round."),
                    BuyPrice = 2,
                    EventAction = new CompositeAction()
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
                            new ScoringAction()
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
                                },
                                Products = -20,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Campfire",
                    Description =
                        new ScalingBoosterDescription(
                            "Gains 0.5x mult for each booster sold. Resets at the end of the act."),
                    BuyPrice = 6,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                XMult = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
                                },
                            },
                            new BoosterScalingAction
                            {
                                XMultChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BoosterSold>(),
                                },
                            },
                            new BoosterResetAction
                            {
                                XMult = 1,
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
                    Name = "Clutch",
                    Description = new LiteralDescription("+10 mult on the last day."),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        PlusMult = 10,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                            TimeCondition.LastDay(),
                        },
                    },
                },
                new()
                {
                    Name = "Overtime",
                    Description = new LiteralDescription("On the last day, Retrigger all buildings"),
                    BuyPrice = 7,
                    EventAction = new RetriggerBuildingAction
                    {
                        RetriggerTimes = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            BuildingTriggerCountCondition.FirstTrigger,
                            TimeCondition.LastDay(),
                        },
                    },
                },
                new()
                {
                    Name = "Sweatshop",
                    Description = new LiteralDescription($"+30 products for each {ColorTag.Red} building"),
                    BuyPrice = 7,
                    EventAction = new ScoringAction
                    {
                        Products = 30,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            new BuildingColorCondition
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
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new DayModifier
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundStart>(),
                                },
                                SetDays = 1,
                            },
                            new ScoringAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
                                    new TimeCondition { Time = 1, TimeMode = TimeCondition.Mode.TotalDays },
                                },
                                XMult = 3,
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
                CreateColorBooster("Purple Boost", ColorTag.Purple),
                CreateColorBooster("Green Boost", ColorTag.Green),
                CreateColorBooster("Red Boost", ColorTag.Red),
                CreateColorBooster("Blue Boost", ColorTag.Blue),
                new()
                {
                    Name = "Crowded",
                    Description = new LiteralDescription("Retrigger all buildings if there are exactly 5 clusters"),
                    BuyPrice = 6,
                    EventAction = new RetriggerBuildingAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            BuildingTriggerCountCondition.FirstTrigger,
                            new CountCondition
                            {
                                TargetType = CountCondition.Target.Cluster,
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
                    Description = new LiteralDescription("+100 products if a cluster has 2 or less buildings"),
                    BuyPrice = 8,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<ClusterScored>(),
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
                    Name = "Painting",
                    Description = new LiteralDescription("+10 mult if all colors are present"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                            new ColorCheckCondition
                            {
                                ForcedColors = ColorTag.All,
                            },
                        },
                        PlusMult = 10,
                    },
                },
                new()
                {
                    Name = "Binary World",
                    Description =
                        new LiteralDescription($"x3 mult if all buildings are {ColorTag.Green} and {ColorTag.Purple}"),
                    BuyPrice = 8,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                            new ColorCheckCondition
                            {
                                BannedColors = new[] { ColorTag.Red, ColorTag.Blue },
                            },
                        },
                        XMult = 3,
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
                            GameEventCondition.Create<DayEnd>(),
                        },
                        PlusMult = 50,
                    },
                },
                new()
                {
                    Name = "Simple",
                    Description = new LiteralDescription("+4 mult"),
                    BuyPrice = 3,
                    EventAction = new ScoringAction
                    {
                        Conditions = new[] { GameEventCondition.Create<DayEnd>() },
                        PlusMult = 4,
                    },
                },
                new()
                {
                    Name = "Sacrifice",
                    Description = new LiteralDescription("+20 mult if 6 or less buildings exist"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
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
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Conditions = new[] { GameEventCondition.Create<RoundStart>() },
                        Actions = new GameAction[]
                        {
                            new DayModifier
                            {
                                Change = -1,
                            },
                            new HandModifier
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
                            "On the start of the round, destroys the booster to the right. Gains +5 mult"),
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new CompositeAction
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundStart>(),
                                },
                                Actions = new GameAction[]
                                {
                                    new DestroyBoosterAction(),
                                    new BoosterScalingAction
                                    {
                                        PlusMultChange = 5,
                                    },
                                },
                            },
                            new ScoringAction
                            {
                                Conditions = new[] { GameEventCondition.Create<DayEnd>() },
                                PlusMult = 0,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Efficiency",
                    Description = new LiteralDescription("+50 products for each day remaining"),
                    BuyPrice = 5,
                    EventAction = new MultipliedScoringAction
                    {
                        MultiplyByDay = true,
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                        },
                    },
                },
                new()
                {
                    Name = "The Collector",
                    Description = new LiteralDescription("+1 mult for each different building"),
                    BuyPrice = 8,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new[] { GameEventCondition.Create<DayEnd>() },
                        PlusMult = 1,
                        MultiplyByUniqueBuildings = true,
                    },
                },
                new()
                {
                    Name = "Cluttered",
                    Description = new LiteralDescription("add double the number of clusters to the mult"),
                    BuyPrice = 4,
                    EventAction = new MultipliedScoringAction
                    {
                        MultiplyByIslandCount = true,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                        },
                        PlusMult = 1,
                    },
                },
                new()
                {
                    Name = "The Banana",
                    Description =
                        new LiteralDescription("+15 mult, 1 in 8 chance to get destroyed at the end of the round"),
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                PlusMult = 15,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
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
                    Description = new LiteralDescription($"+4 mult for each {Category.Cat1} building"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        PlusMult = 4,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
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
                    Description = new LiteralDescription($"+50 products for each {Category.Cat2} building"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Products = 50,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            new BuildingCategoryCondition
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
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        PlusMult = 4,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingRemainedInHand>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat5 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Investment",
                    Description =
                        new LiteralDescription("$1 for each building remained in hand at the end of the day."),
                    BuyPrice = 6,
                    EventAction = new ChangeMoneyAction
                    {
                        IsImmediate = true,
                        Change = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingRemainedInHand>(),
                        },
                    },
                },
                new()
                {
                    Name = "Ice Cream",
                    Description = new ScalingBoosterDescription("+100 products, loses 5 products every day."),
                    BuyPrice = 4,
                    EventAction = new CompositeAction
                    {
                        Conditions = new[] { GameEventCondition.Create<DayEnd>() },
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
                        },
                    },
                },
                new()
                {
                    Name = "Normie",
                    Description = new LiteralDescription("x5 mult if all buildings are EXACTLY the same"),
                    BuyPrice = 6,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                            new SameBuildingsCondition(),
                        },
                    },
                },
                // new()
                // {
                //     Name = "Net",
                //     Description = new ScalingBoosterDescription("Gain +1 mult whenever a random event does not happen"),
                // },
                // new()
                // {
                //     Name = "For Profit",
                //     Description = new LiteralDescription("Double the chance of a world booster spawning"),
                //     BuyPrice = 5,
                //     EventAction = new ModifyWorldBoosterSpawnAction()
                //     {
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<PropertiesRestored>(),
                //         },
                //         FactorMultiplier = 2,
                //     },
                // },
                // new()
                // {
                //     Name = "For Fun",
                //     Description = new LiteralDescription("Double the total number of world boosters"),
                //     BuyPrice = 5,
                //     EventAction = new ModifyWorldBoosterSpawnAction()
                //     {
                //         Conditions = new IGameCondition[]
                //         {
                //             GameEventCondition.Create<PropertiesRestored>(),
                //         },
                //         CountMultiplier = 2,
                //     },
                // },
                new()
                {
                    Name = "Late Bloomer",
                    Description =
                        new ScalingBoosterDescription(
                            "0.5x mult at the end of the day. After 5 rounds, turns into x5 mult"),
                    BuyPrice = 3,
                    SellPrice = 2,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction()
                            {
                                Conditions = new[] { GameEventCondition.Create<DayEnd>() },
                                XMult = 0.5,
                            },
                            new BoosterScalingAction()
                            {
                                XMultChange = 4.5,
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
                    Name = "Acrobat",
                    Description = new LiteralDescription("x3 mult on the final day of the round"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                            TimeCondition.LastDay(),
                        },
                        XMult = 3,
                    },
                },
                new()
                {
                    Name = "Stuntman",
                    Description = new LiteralDescription("+250 products, -2 hand size"),
                    BuyPrice = 6,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                Products = 250,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
                                },
                            },
                            new HandModifier
                            {
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<RoundStart>(),
                                },
                                Change = -2,
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Shoot the moon",
                    Description =
                        new LiteralDescription($"+10 mult for each {Category.Cat4} building remaining in hand"),
                    BuyPrice = 5,
                    EventAction = new ScoringAction
                    {
                        PlusMult = 10,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingRemainedInHand>(),
                            new BuildingCategoryCondition
                            {
                                Categories = new[] { Category.Cat4 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Capitalist",
                    Description = new ScalingBoosterDescription("+2 mult for every $5 you have"),
                    BuyPrice = 7,
                    EventAction = new MultipliedScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                        },
                        PlusMult = 2,
                        PerMoney = 5,
                    },
                },
                new()
                {
                    Name = "Bull",
                    Description = new ScalingBoosterDescription("+5 products for every $5 you have"),
                    BuyPrice = 7,
                    EventAction = new MultipliedScoringAction()
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                        },
                        Products = 5,
                        PerMoney = 5,
                    },
                },
                new()
                {
                    Name = "Rural",
                    Description = new ScalingBoosterDescription(
                        $"Gains 0.5x mult for every {Category.Cat1} building remained in hand. Resets at the end of the round"),
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                XMult = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
                                },
                            },
                            new BoosterScalingAction
                            {
                                XMultChange = 0.5,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<BuildingRemainedInHand>(),
                                    new BuildingCategoryCondition
                                    {
                                        Categories = new[] { Category.Cat1 },
                                    },
                                },
                            },
                            new BoosterResetAction
                            {
                                XMult = 1,
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
                    Name = "Gems",
                    Description = new LiteralDescription($"{Category.Cat4} buildings earn $1 when scored"),
                    BuyPrice = 4,
                    EventAction = new ChangeMoneyAction()
                    {
                        Change = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<BuildingScored>(),
                            new BuildingCategoryCondition()
                            {
                                Categories = new[] { Category.Cat4 },
                            },
                        },
                    },
                },
                new()
                {
                    Name = "Swash",
                    Description =
                        new ScalingBoosterDescription("Adds the sell value of all other owned boosters to the mult"),
                    BuyPrice = 4,
                    EventAction = new MultipliedScoringAction()
                    {
                        Conditions = new[] { GameEventCondition.Create<DayEnd>() },
                        PlusMult = 0,
                        MultiplyBySellValueOfBoosters = true,
                    },
                },
                new()
                {
                    Name = "Popcorn",
                    Description = new ScalingBoosterDescription("+20 mult, -4 mult per round played"),
                    BuyPrice = 5,
                    EventAction = new CompositeAction
                    {
                        Actions = new GameAction[]
                        {
                            new ScoringAction
                            {
                                PlusMult = 20,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
                                },
                            },
                            new BoosterScalingAction
                            {
                                PlusMultChange = -4,
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
                    Name = "Juggler",
                    Description = new LiteralDescription("+1 hand size"),
                    BuyPrice = 4,
                    EventAction = new HandModifier
                    {
                        Change = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<RoundStart>(),
                        },
                    },
                },
                new()
                {
                    Name = "Lenient",
                    Description = new LiteralDescription("+1 day"),
                    BuyPrice = 4,
                    EventAction = new DayModifier
                    {
                        Change = 1,
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<RoundStart>(),
                        },
                    },
                },
                new()
                {
                    Name = "Way too blue",
                    Description = new ScalingBoosterDescription("+2 products for each remaining card in the deck"),
                    BuyPrice = 5,
                    EventAction = new MultipliedScoringAction
                    {
                        Conditions = new IGameCondition[]
                        {
                            GameEventCondition.Create<DayEnd>(),
                        },
                        Products = 2,
                        MultiplyByRemainingCards = true,
                    },
                },
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
                                XMult = 1,
                                Conditions = new IGameCondition[]
                                {
                                    GameEventCondition.Create<DayEnd>(),
                                },
                            },
                            new BoosterScalingAction
                            {
                                XMultChange = 0.1,
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
                }
            };
        }

        private static BoosterCard CreateColorBooster(string name, ColorTag color)
            => new()
            {
                Name = name,
                Description = new LiteralDescription($"+3 mult when {color} buildings score"),
                BuyPrice = 3,
                EventAction = new ScoringAction()
                {
                    PlusMult = 3,
                    Conditions = new IGameCondition[]
                    {
                        GameEventCondition.Create<BuildingScored>(),
                        new BuildingColorCondition()
                        {
                            Colors = new[] { color },
                        },
                    },
                },
            };
    }
}