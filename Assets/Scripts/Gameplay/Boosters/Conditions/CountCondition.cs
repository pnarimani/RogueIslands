using System;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Sources;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class CountCondition : IGameConditionWithSource<int>
    {
        public enum Mode
        {
            Less,
            More,
            Equal,
            Even,
            Odd,
            PowerOfTwo,
        }
        
        public ISource<int> Source { get; set; }
        public Mode ComparisonMode { get; set; }
        public int Value { get; set; }
        
        public bool Evaluate(IBooster booster)
        {
            if (Source == null)
                throw new InvalidOperationException($"Source is null");
            
            var count = Source.Get(booster).First();

            return ComparisonMode switch
            {
                Mode.Less => count < Value,
                Mode.More => count > Value,
                Mode.Equal => count == Value,
                Mode.Even => count % 2 == 0,
                Mode.Odd => count % 2 == 1,
                Mode.PowerOfTwo => (count & (count - 1)) == 0,
                _ => false,
            };
        }
    }
}