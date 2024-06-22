namespace RogueIslands.Boosters.Actions
{
    public class ChangeMoneyAction : GameAction
    {
        public bool IsImmediate { get; set; }
        public int Change { get; set; }
    }
}