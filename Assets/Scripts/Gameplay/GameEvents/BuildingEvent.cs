using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BuildingEvent : IGameEvent
    {
        public Building Building { get; set; }
    }
}