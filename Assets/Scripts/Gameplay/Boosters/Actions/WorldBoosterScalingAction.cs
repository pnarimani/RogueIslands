namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class WorldBoosterScalingAction : GameAction
    {
        public double? StartingProducts { get; set; }
        public double? StartingMultiplier { get; set; }
        public double? StartingXMult { get; set; }
        
        public double? ProductChangePerBuildingInside { get; set; }
        public double? PlusMultChangePerBuildingInside { get; set; }
        public double? XMultChangePerBuildingInside { get; set; }
        
        public double? ProductChangePerBuildingOutside { get; set; }
        public double? PlusMultChangePerBuildingOutside { get; set; }
        public double? XMultChangePerBuildingOutside { get; set; }
    }
}