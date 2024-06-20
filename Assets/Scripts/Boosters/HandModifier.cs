namespace RogueIslands.Boosters
{
    public class HandModifier : GameAction
    { 
        public int Change { get; set;}
        public int? SetHandSize { get; set; }
        public bool ShouldSetToDefault { get; set; }
    }
}