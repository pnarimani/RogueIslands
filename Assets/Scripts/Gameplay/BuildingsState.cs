using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;
using Unity.Mathematics;

namespace RogueIslands.Gameplay
{
    public class BuildingsState
    {
        public Random[] ShufflingRandom;
        public List<Building> Deck;
        public int HandPointer;
        public List<Building> All;
    }
}