using System.Collections.Generic;
using System.Linq;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class CountItems<T> : ISource<int>
    {
        public ISource<T> Source { get; set; }
        
        public IEnumerable<int> Get(IBooster booster)
        {
            yield return Source.Get(booster).Count();
        }
    }
}