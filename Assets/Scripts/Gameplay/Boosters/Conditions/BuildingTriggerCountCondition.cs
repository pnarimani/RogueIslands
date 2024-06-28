namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingTriggerCountCondition : IGameCondition
    {
        public int TriggerCount { get; set; }
        
        public static BuildingTriggerCountCondition FirstTrigger => new BuildingTriggerCountCondition { TriggerCount = 1 };
    }
}