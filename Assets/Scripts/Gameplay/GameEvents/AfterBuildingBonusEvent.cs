using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class AfterBuildingBonusEvent : BuildingEvent
    {
        public Building PlacedBuilding { get; set; }
    }
}