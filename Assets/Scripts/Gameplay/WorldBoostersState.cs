using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public class WorldBoostersState
    {
        public RogueRandom SpawnRandom, SelectionRandom, PositionRandom;
        public PowerDistribution SpawnDistribution;
        public double SpawnCount;
        public List<WorldBooster> SpawnedBoosters = new();
        public List<WorldBooster> All;
    }
}