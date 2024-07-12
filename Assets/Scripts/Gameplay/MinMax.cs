using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public readonly struct MinMax
    {
        public readonly int Min;
        public readonly int Max;

        public MinMax(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int GetInRange(RandomForAct random) 
            => random.NextInt(Min, Max);

        public static MinMax operator *(MinMax minMax, int multiplier) =>
            new(minMax.Min * multiplier, minMax.Max * multiplier);
    }
}