using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class SizePlayCountSource : ISource<int>
    {
        public ISource<BuildingSize> Size { get; set; }

        public IEnumerable<int> Get(GameState state, IBooster booster)
        {
            foreach (var size in Size.Get(state, booster))
            {
                state.Metadata.RoundSizePlayCount.TryGetValue(size, out var value);
                yield return value;
            }
        }
    }
}