using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class OwnedBoosters : ISource<BoosterCard>
    {
        public IEnumerable<BoosterCard> Get(GameState state, IBooster booster)
        {
            return state.Boosters;
        }
    }
}