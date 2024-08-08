using System.Collections.Generic;
using System.Linq;
using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class TotalBuildingsPlayedThisRound : ISource<int>
    {
        public IEnumerable<int> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            return new[] { state.Metadata.RoundSizePlayCount.Values.Sum() };
        }
    }
}