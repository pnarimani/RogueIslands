namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class BoosterScalingAction : GameAction
    {
        public double? ProductChange { get; set; }
        public double? MultiplierChange { get; set; }
        
        public bool OneTime { get; set; }
        public bool HasTriggered { get; set; }
        public int? Delay { get; set; }
        public int Progress { get; set; }
    }
}