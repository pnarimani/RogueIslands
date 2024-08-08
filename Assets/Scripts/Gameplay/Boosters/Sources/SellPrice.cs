using System.Collections.Generic;
using System.Linq;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class SellPrice<T> : ISource<int>
    {
        public ISource<T> Source { get; set; }

        public IEnumerable<int> Get(IBooster booster)
        {
            yield return Source.Get(booster).Sum(c =>
            {
                if (c is IPurchasableItem item)
                    return item.SellPrice;
                return 0;
            });
        }
    }
}