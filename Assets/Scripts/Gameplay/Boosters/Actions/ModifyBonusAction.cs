namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ModifyBonusAction : GameAction
    {
        public double? Add { get; set; }
        public double? Multiplier { get; set; }
        public double? ColorMultiplier { get; set; }
        public double? CategoryMultiplier { get; set; }
        public double? SizeMultiplier { get; set; }
    }
}