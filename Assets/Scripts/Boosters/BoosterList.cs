using System.Collections.Generic;

namespace RogueIslands.Boosters
{
    public static class BoosterList
    {
        public static IReadOnlyList<Booster> Get()
        {
            return new Booster[]
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
                    EventAction = new MultiplierModifierAction()
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
                        },
                        XMult = 2,
                    },
                },
                new()
                {
                    Name = "Hiker",
                    Description = "Permanently add +10 product to played buildings",
                    EventAction = new PermanentUpgradeAction()
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
            };
        }
    }
}