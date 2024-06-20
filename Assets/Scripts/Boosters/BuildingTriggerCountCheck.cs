namespace RogueIslands.Boosters
{
    public class BuildingTriggerCountCheck : IGameCondition
    {
        public int TriggerCount { get; set; }
        
        public static BuildingTriggerCountCheck FirstTrigger => new BuildingTriggerCountCheck { TriggerCount = 1 };
    }
}