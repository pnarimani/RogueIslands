namespace RogueIslands
{
    public class ScoringState
    {
        public double Products { get; set; }
        public double Multiplier { get; set; } = 1;
        public Island CurrentScoringIsland { get; set; }
        public Building CurrentScoringBuilding { get; set; }
    }
}