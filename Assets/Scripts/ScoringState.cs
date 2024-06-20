namespace RogueIslands
{
    public class ScoringState
    {
        public double Products { get; set; }
        public double Multiplier { get; set; } = 1;
        public Island SelectedIsland { get; set; }
        public Building SelectedBuilding { get; set; }
        public int SelectedBuildingTriggerCount { get; set; }
    }
}