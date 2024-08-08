using System.Collections.Generic;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class BuildingsInDeck : ISource<Building>
    {
        public IEnumerable<Building> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            return state.Buildings.Deck;
        }
    }
}