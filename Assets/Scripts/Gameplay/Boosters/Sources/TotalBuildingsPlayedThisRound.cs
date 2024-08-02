using System.Collections.Generic;
using System.Linq;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class TotalBuildingsPlayedThisRound : ISource<int>
    {
        public IEnumerable<int> Get(GameState state, IBooster booster)
        {
            return new[] { state.Metadata.RoundSizePlayCount.Values.Sum() };
        }
    }
}