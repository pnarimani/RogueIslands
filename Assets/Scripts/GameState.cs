using System.Collections.Generic;
using RogueIslands.Boosters;
using RogueIslands.GameEvents;
using Unity.Mathematics;

namespace RogueIslands
{
    public class GameState
    {
        public const int TotalRounds = 4;
        public const int TotalActs = 5;

        public int Day;
        public int Round;
        public int Act;

        public int DefaultTotalDays = 4;
        public int TotalDays = 4;

        public int DefaultHandSize = 4;
        public int HandSize = 4;

        public double CurrentScore;

        public double[] AllRequiredScores;
        public double RequiredScore => AllRequiredScores[Act * TotalRounds + Round];

        public int Money = 4;
        public int MoneyPayoutPerWeek = 4;
        public List<MoneyChange> MoneyChanges = new();

        public IGameEvent CurrentEvent;
        public ScoringState ScoringState;

        public List<Cluster> Clusters = new();
        public List<Building> BuildingsInHand = new();
        public BuildingDeck BuildingDeck;
        public List<Building> AvailableBuildings;

        public int MaxBoosters = 5;
        public List<BoosterCard> Boosters = new();
        public List<BoosterCard> AvailableBoosters;

        public ShopState Shop;

        public GameResult Result;

        public Random WorldBoosterRandom;

        public List<WorldBooster> WorldBoosters = new();
        public List<WorldBooster> AvailableWorldBoosters;
    }
}