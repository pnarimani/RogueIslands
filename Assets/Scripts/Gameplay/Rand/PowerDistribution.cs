using System;

namespace RogueIslands.Gameplay.Rand
{
    public struct PowerDistribution
    {
        public double Factor, Power;

        public double Calculate(double x)
        {
            return Math.Pow(x, Power) * Factor;
        }
        
        public double GetNextDouble(RandomForAct random)
        {
            return Math.Clamp(Calculate(random.NextDouble()), 0, 1);
        }
    }
}