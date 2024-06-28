using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay
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
        public IEnumerable<Building> BuildingsInHand => Buildings.Deck
            .Skip(Buildings.HandPointer - HandSize)
            .Take(HandSize)
            .Where(b => !b.IsPlacedDown());
        public IEnumerable<Building> PlacedDownBuildings => Buildings.Deck
            .Where(b => b.IsPlacedDown());

        public int MaxBoosters = 5;
        public List<BoosterCard> Boosters = new();
        public List<BoosterCard> AvailableBoosters;

        public ShopState Shop;
        public DeckBuildingState DeckBuilding; 

        public GameResult Result;

        public WorldBoostersState WorldBoosters;
    }
}