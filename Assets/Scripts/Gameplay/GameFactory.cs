using System;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public static class GameFactory
    {
        public static GameState NewGame(Seed seed)
        {
            const int handSize = 6;

            var seedRandom = new SeedRandom((uint)seed.Value.GetHashCode());
            
            var buildingBlueprints = DefaultBuildingsList.Get();
            var deck = DefaultBuildingsList.Get();
            deck.Shuffle(seedRandom);
            deck = deck.GetRange(0, handSize);
            foreach (var building in deck)
            {
                building.Id = BuildingId.NewBuildingId();
            }

            return new GameState()
            {
                Seed = seed,
                AllRequiredScores = GetScoringRequirements(),
                CurrentEvent = new ActStartEvent(),
                Buildings = new BuildingsState()
                {
                    All = buildingBlueprints,
                    Deck = deck,
                    ShufflingRandom = seedRandom.NextRandom(),
                },
                AvailableBoosters = BoosterList.Get(),
                HandSize = handSize,
                CardPackSelectionRandom = seedRandom.NextRandom(),
                CardSelectionRandom = seedRandom.NextRandom(),
                Consumables = new ConsumablesState()
                {
                    BuildingSelectionRandom = seedRandom.NextRandom(),
                    AllConsumables = ConsumableList.Get(),
                },
                Shop = new ShopState
                {
                    StartingRerollCost = 5,
                    BoosterSpawn = seedRandom.NextRandom(),
                    CardPackSpawn = seedRandom.NextRandom(),
                    DeduplicationRandom = seedRandom.NextRandom(),
                    BuildingSpawn = seedRandom.NextRandom(),
                    SelectionRandom = seedRandom.NextRandom(),
                    RarityRandom = seedRandom.NextRandom(),
                    CurrentRerollCost = 5,
                },
            };
        }



        private static double[] GetScoringRequirements()
        {
            var reqPerAct = new double[]
            {
                20,
                400,
                3000,
                25000,
            };

            var result = new double[GameState.RoundsPerAct * GameState.TotalActs];
            for (var i = 0; i < GameState.TotalActs; i++)
            {
                for (var j = 0; j < GameState.RoundsPerAct; j++)
                {
                    var x = i * GameState.RoundsPerAct + j;
                    var mult = 1 + j * 0.5;
                    var score = reqPerAct[i] * mult;

                    result[x] = Math.Round(score);
                }
            }

            return result;
        }
    }
}