using System.Collections.Generic;
using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class MoneyAmount : ISource<int>
    {
        public int DivideBy { get; set; } = 1;
        
        public IEnumerable<int> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            yield return state.Money / DivideBy;
        }
    }
}