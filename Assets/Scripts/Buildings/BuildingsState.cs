using System.Collections.Generic;
using Unity.Mathematics;

namespace RogueIslands.Buildings
{
    public class BuildingsState
    {
        public Random[] ShufflingRandom;
        public List<Cluster> Clusters = new();
        public List<Building> Deck;
        public int HandPointer;
        public List<Building> All;
    }
}