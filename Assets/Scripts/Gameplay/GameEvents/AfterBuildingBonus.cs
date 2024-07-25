using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class AfterBuildingBonus : BuildingEvent
    {
        public Building PlacedBuilding { get; set; }
    }
}