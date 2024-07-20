using JetBrains.Annotations;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BuildingEvent : IGameEvent
    {
        [NotNull]
        public Building Building { get; set; }
    }
}