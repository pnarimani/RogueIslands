using System.Collections.Generic;
using RogueIslands.Gameplay.DeckBuilding;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class DeckBuildingState
    {
        public List<Consumable> AllConsumables { get; set; }
        public RogueRandom BuildingSelectionRandom;
    }
}