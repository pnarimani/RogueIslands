using System.Collections.Generic;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public class GameState
    {
        public const int TotalWeeks = 4;
        public const int TotalMonths = 5;

        public int Day { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }

        public int DefaultTotalDays { get; set; } = 4;
        public int TotalDays { get; set; } = 4;

        public int DefaultHandSize { get; set; } = 4;
        public int HandSize { get; set; } = 4;
        
        public double CurrentScore { get; set; }
        
        public double[] AllRequiredScores { get; set; }
        public double RequiredScore => AllRequiredScores[Month * TotalWeeks + Week];
        
        public int Money { get; set; } = 4;
        public int MoneyPayoutPerWeek { get; set; } = 4;
        public List<MoneyChange> MoneyChanges { get; set; } = new();

        public string CurrentEvent { get; set; }
        public ScoringState ScoringState { get; set; }

        public List<Island> Islands { get; set; } = new();
        public List<Building> BuildingsInHand { get; set; } = new();
        public List<Building> BuildingDeck { get; set; }
        public List<Building> AvailableBuildings { get; set; }

        public int MaxBoosters { get; set; } = 5;
        public List<Booster> Boosters { get; set; } = new();
        public List<Booster> AvailableBoosters { get; set; }
        
        public ShopState Shop { get; set; }
        
        public GameResult Result { get; set; }

        public List<IWorldBooster> WorldBoosters { get; set; } = new();
    }
}