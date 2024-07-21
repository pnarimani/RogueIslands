using System;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public readonly struct MinMax
    {
        public readonly double Min;
        public readonly double Max;

        public MinMax(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double GetNextDoubleInRange(RandomForAct random)
            => random.NextDouble(Min, Max);

        public int GetNextIntInRange(RandomForAct random)
            => (int)Math.Round(random.NextDouble(Min, Max));

        public static MinMax operator *(MinMax minMax, int multiplier) =>
            new(minMax.Min * multiplier, minMax.Max * multiplier);

        public bool Contains(double value) 
            => value >= Min && value <= Max;
    }
}