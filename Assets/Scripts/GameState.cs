using System.Collections.Generic;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public class GameState
    {
        public const int TotalWeeks = 4;
        
        public int Day { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        
        public int TotalDays { get; set; }
        public int TotalMonths { get; set; } = 5;
        
        public double CurrentScore { get; set; }
        public double RequiredScore { get; set; }
        
        public int Energy { get; set; }
        public int Money { get; set; }

        public string CurrentEvent { get; set; }
        public ScoringState ScoringState { get; set; }

        public List<Island> PlacedBuildings { get; set; } = new();
        public List<Building> BuildingsInHand { get; set; } = new();
        public List<Building> AvailableBuildings { get; set; }

        public int MaxBoosters { get; set; } = 5;
        public List<Booster> Boosters { get; set; }
        public List<Booster> AvailableBoosters { get; set; }
    }
}