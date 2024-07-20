namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class MultipliedScoringAction : ScoringAction
    {
        public bool MultiplyByDay { get; set; }
        public bool MultiplyByIslandCount { get; set; }
        public bool MultiplyByUniqueBuildings { get; set; }
        public int? PerMoney { get; set; }
        public bool MultiplyBySellValueOfBoosters { get; set; }
        public bool MultiplyByRemainingCards { get; set; }
        public bool MultiplyByRedBuildingsInRange { get; set; }
        public bool MultiplyByIdenticalBuildingsInRange { get; set; }
    }
}