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
        public int TotalDiscards = 3;
        public int DiscardsLeft = 3;

        public double TransientScore { get; set; }
        public double CurrentScore { get; set; }
        public double[] AllRequiredScores { get; set; }

        public int Money = 4;
        public int MoneyPayoutPerRound = 4;
        public List<MoneyChange> MoneyChanges = new();

        public IGameEvent CurrentEvent;

        public BuildingsState Buildings;

        public IEnumerable<Building> BuildingsInHand => Buildings.Deck
            .Where(b => !b.IsPlacedDown(this))
            .Take(HandSize);

        public IEnumerable<Building> DeckPeek => Buildings.Deck
            .Where(b => !b.IsPlacedDown(this))
            .Skip(HandSize)
            .Take(3);

        public IEnumerable<Building> PlacedDownBuildings => Buildings.PlacedDownBuildings;

        public int MaxBoosters = 5;
        public List<BoosterCard> Boosters = new();
        public List<BoosterCard> AvailableBoosters;

        public ShopState Shop;
        public ConsumablesState Consumables;

        public GameResult Result;

        public WorldBoostersState WorldBoosters;
    }
}