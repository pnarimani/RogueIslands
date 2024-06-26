using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters;
using RogueIslands.Buildings;
using RogueIslands.GameEvents;

namespace RogueIslands
{
    public class GameState
    {
        public const int RoundsPerAct = 4;
        public const int TotalActs = 5;

        public int Day;
        public int Round;
        public int Act;

        public int TotalDays = 4;
        public int HandSize = 4;

        public double CurrentScore;

        public double[] AllRequiredScores;

        public int Money = 4;
        public int MoneyPayoutPerRound = 4;
        public List<MoneyChange> MoneyChanges = new();

        public IGameEvent CurrentEvent;
        public ScoringState ScoringState;

        public BuildingsState Buildings;
        public List<Cluster> Clusters => Buildings.Clusters;
        public IEnumerable<Building> BuildingsInHand => Buildings.Deck.Skip(Buildings.HandPointer).Take(HandSize);
        public BuildingsState BuildingDeck => Buildings;

        public int MaxBoosters = 5;
        public List<BoosterCard> Boosters = new();
        public List<BoosterCard> AvailableBoosters;

        public ShopState Shop;

        public GameResult Result;

        public WorldBoostersState WorldBoosters;
    }
}