using System.Collections.Generic;
using RogueIslands.Boosters;

namespace RogueIslands
{
    public class GameState
    {
        public double CurrentScore { get; set; }
        public double RequiredScore { get; set; }
        
        public int Energy { get; set; }
        public int Money { get; set; }

        public string CurrentEvent { get; set; }
        public ScoringState ScoringState { get; set; }

        public List<PlacedBuilding> PlacedBuildings { get; set; } = new();
        public List<Building> BuildingsInHand { get; set; } = new();

        public int MaxBoosters { get; set; } = 5;
        public List<Booster> Boosters { get; set; }
    }
}