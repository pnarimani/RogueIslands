using System.Collections.Generic;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class SizePlayCountSource : ISource<int>
    {
        public ISource<BuildingSize> Size { get; set; }

        public IEnumerable<int> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            foreach (var size in Size.Get(booster))
            {
                state.Metadata.RoundSizePlayCount.TryGetValue(size, out var value);
                yield return value;
            }
        }
    }
}