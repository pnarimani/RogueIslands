using System.Collections.Generic;
using RogueIslands.Buildings;
using Unity.Mathematics;

namespace RogueIslands
{
    public class BuildingsState
    {
        public Random[] ShufflingRandom;
        public List<Building> Deck;
        public int HandPointer;
        public List<Building> All;
    }
}