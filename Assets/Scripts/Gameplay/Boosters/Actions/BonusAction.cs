namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class BonusAction : GameAction
    {
        public double? Addition { get; set; }
        public double? Multiplier { get; set; }
        public double? ColorMultiplier { get; set; }
        public double? CategoryMultiplier { get; set; }
        public double? SizeMultiplier { get; set; }
    }
}