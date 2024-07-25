using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.GameEvents
{
    public class BuildingBonus : BuildingEvent
    {
        public Building PlacedBuilding { get; set; }
    }
}