namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class BonusScalingAction : GameAction
    {
        public double? AdditionChange { get; set; }
        public double? MultiplierChange { get; set; }
        public double? ColorMultiplierChange { get; set; }
        public double? CategoryMultiplierChange { get; set; }
        public double? SizeMultiplierChange { get; set; }
            
        public bool OneTime { get; set; }
        public bool HasTriggered { get; set; }
        public int? Delay { get; set; }
        public int Progress { get; set; }
    }
}