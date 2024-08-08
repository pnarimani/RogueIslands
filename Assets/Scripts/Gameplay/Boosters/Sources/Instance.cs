using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class Instance<T> : ISource<T>
    {
        public T Value { get; set; }

        public IEnumerable<T> Get(IBooster booster)
        {
            yield return Value;
        }
    }
}