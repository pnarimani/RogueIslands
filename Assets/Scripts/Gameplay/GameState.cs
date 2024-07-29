using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class GameState
    {
        public const int RoundsPerAct = 5;
        public const int TotalActs = 4;
        
        public SeedRandom SeedRandom { get; set; }

        public int Round;
        public int Act;

        public int HandSize = 4;

        public int Money = 4;
        public int MoneyPayoutPerRound = 4;
        public List<MoneyChange> MoneyChanges = new();

        public ScoreState Score { get; set; } = new();

        public double TransientScore
        {
            get => Score.TransientScore;
            set => Score.TransientScore = value;
        }

        public double CurrentScore
        {
            get => Score.CurrentScore;
            set => Score.CurrentScore = value;
        }

        public double[] AllRequiredScores
        {
            get => Score.AllRequiredScores;
            set => Score.AllRequiredScores = value;
        }

        public IGameEvent CurrentEvent;

        public BuildingsState Buildings;

        public IEnumerable<Building> BuildingsInHand => Buildings.Deck
            .Take(HandSize);

        public IEnumerable<Building> DeckPeek => Buildings.Deck
            .Skip(HandSize)
            .Take(3);

        public IEnumerable<Building> PlacedDownBuildings => Buildings.PlacedDownBuildings;

        public RogueRandom CardPackSelectionRandom;
        public RogueRandom CardSelectionRandom;
        public int CardPerPack = 12;

        public int MaxBoosters = 5;
        public List<BoosterCard> Boosters = new();
        public List<BoosterCard> AvailableBoosters;

        public ShopState Shop;
        public ConsumablesState Consumables;

        public GameResult Result;

        public WorldBoostersState WorldBoosters;

        public Dictionary<string, RogueRandom> Randoms { get; set; } = new();
    }
}