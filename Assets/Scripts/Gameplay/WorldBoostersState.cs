using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class WorldBoostersState
    {
        public RogueRandom CountRandom, SpawnRandom, SelectionRandom, PositionRandom;
        public float SpawnChance;
        public MinMax Count;
        public List<WorldBooster> SpawnedBoosters = new();
        public List<WorldBooster> All;
    }
}