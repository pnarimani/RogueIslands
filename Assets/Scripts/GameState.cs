using System.Collections.Generic;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public class GameState
    {
        public const int TotalWeeks = 4;
        public const int TotalMonths = 5;

        public int Day { get; set; } = 1;
        public int Week { get; set; }
        public int Month { get; set; }

        public int TotalDays { get; set; } = 3;
        
        public double CurrentScore { get; set; }
        
        public double[] AllRequiredScores { get; set; }
        public double RequiredScore => AllRequiredScores[Month * TotalWeeks + Week];

        public int Energy { get; set; } = 4;
        public int Money { get; set; } = 4;

        public string CurrentEvent { get; set; }
        public ScoringState ScoringState { get; set; }

        public List<Island> Islands { get; set; } = new();
        public List<Building> BuildingsInHand { get; set; } = new();
        public List<Building> AvailableBuildings { get; set; }

        public int MaxBoosters { get; set; } = 5;
        public List<Booster> Boosters { get; set; } = new();
        public List<Booster> AvailableBoosters { get; set; }
        
        public GameResult Result { get; set; }
    }
}