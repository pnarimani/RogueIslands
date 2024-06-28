using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;
using Unity.Mathematics;

namespace RogueIslands.Gameplay
{
    public class WorldBoostersState
    {
        public Random CountRandom, SpawnRandom, SelectionRandom, PositionRandom;
        public float SpawnChance;
        public MinMax Count;
        public List<WorldBooster> SpawnedBoosters = new();
        public List<WorldBooster> All;
    }
}