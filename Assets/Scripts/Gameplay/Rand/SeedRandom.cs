using Unity.Mathematics;

namespace RogueIslands.Gameplay.Rand
{
    public class SeedRandom
    {
        private Random _randomGenerator;

        public SeedRandom(uint initialSeed) => _randomGenerator = new Random(initialSeed);

        public RogueRandom NextRandom()
            => new(_randomGenerator.NextUInt());

        public int NextInt(int max) => _randomGenerator.NextInt(max);
    }
}