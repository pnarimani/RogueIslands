using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class BuildingsState
    {
        public RogueRandom ShufflingRandom;
        public List<Building> PlacedDownBuildings = new();
        public List<Building> Deck;
        public int HandPointer;
        public List<Building> All;
    }
}