using System.Collections.Generic;

namespace RogueIslands
{
    public class ScoringState
    {
        public double Products { get; set; }
        public double Multiplier { get; set; }
        public Island CurrentScoringIsland { get; set; }
        public PlacedBuilding CurrentScoringBuilding { get; set; }

        public Dictionary<string, object> CustomDate { get; set; }
    }
}