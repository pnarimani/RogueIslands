namespace RogueIslands.GameEvents
{
    public class BuildingEvent : IGameEvent
    {
        public Building Building { get; set; }
        public int TriggerCount { get; set; }
    }
}