using RogueIslands.Buildings;

namespace RogueIslands.GameEvents
{
    public class BuildingScored : BuildingEvent
    {
        public Cluster Cluster { get; set; }
    }
}