using System.Collections.Generic;
using RogueIslands.Gameplay.DeckBuilding;
using Unity.Mathematics;

namespace RogueIslands.Gameplay
{
    public class DeckBuildingState
    {
        public List<Consumable> AllConsumables { get; set; }
        public Random BuildingSelectionRandom;
    }
}