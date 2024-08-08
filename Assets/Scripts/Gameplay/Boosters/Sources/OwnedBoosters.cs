using System.Collections.Generic;
using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class OwnedBoosters : ISource<BoosterCard>
    {
        public IEnumerable<BoosterCard> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            return state.Boosters;
        }
    }
}