namespace RogueIslands.Gameplay.Rand
{
    public readonly struct RandomForAct
    {
        private readonly RogueRandom _random;
        private readonly int _act;

        public RandomForAct(RogueRandom random, int act)
        {
            _act = act;
            _random = random;
        }
        
        public int NextInt(int max)
        {
            return _random.GetActRandom(_act).NextInt(max);
        }
        
        public float NextFloat()
        {
            return _random.GetActRandom(_act).NextFloat();
        }
        
        public float NextFloat(float max)
        {
            return _random.GetActRandom(_act).NextFloat(max);
        }
        
        public float NextFloat(float min, float max)
        {
            return _random.GetActRandom(_act).NextFloat(min, max);
        }

        public int NextInt(int min, int max)
        {
            return _random.GetActRandom(_act).NextInt(min, max);
        }

        public double NextDouble(double min, double max)
        {
            return _random.GetActRandom(_act).NextDouble(min, max);
        }
        
        public double NextDouble()
        {
            return _random.GetActRandom(_act).NextDouble();
        }

        public uint NextUInt()
        {
            return _random.GetActRandom(_act).NextUInt();
        }
    }
}