﻿using Unity.Mathematics;

namespace RogueIslands
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

        public int GetRandom(Random random)
        {
            return random.NextInt(Min, Max);
        }
    }
}