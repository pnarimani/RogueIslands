namespace RogueIslands.Boosters
{
    public class DayModifier : GameAction
    { 
        public int Change { get; set;}
        public int? SetDays { get; set; }
        public bool ShouldSetToDefault { get; set; }
    }
}