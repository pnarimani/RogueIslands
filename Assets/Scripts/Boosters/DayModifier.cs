namespace RogueIslands.Boosters
{
    public class DayModifier : GameAction
    { 
        public int ChangeDays { get; set;}
        public int? SetDays { get; set; }
        public bool ShouldSetToDefault { get; set; }
    }
}