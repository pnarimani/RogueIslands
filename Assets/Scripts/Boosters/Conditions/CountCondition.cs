namespace RogueIslands.Boosters
{
    public class CountCondition : IGameCondition
    {
        public enum Target
        {
            Buildings,
            Cluster,
            BuildingsInAnyIsland,
            BuildingsInScoringIsland,
        }

        public enum Mode
        {
            Less,
            More,
            Equal,
            Even,
            Odd,
            PowerOfTwo,
        }
        
        public Target TargetType { get; set; }
        public Mode ComparisonMode { get; set; }
        public int Value { get; set; }
    }
}