using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BuildingBonusEvent : BuildingEvent
    {
        public Building PlacedBuilding { get; set; }
    }
}