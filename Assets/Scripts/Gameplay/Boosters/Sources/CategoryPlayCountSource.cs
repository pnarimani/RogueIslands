using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class CategoryPlayCountSource : ISource<int>
    {
        public Category Category { get; set; }

        public IEnumerable<int> Get(GameState state, IBooster booster)
        {
            state.Metadata.RunCategoryPlayCount.TryGetValue(Category, out var value);
            return new[] { value };
        }
    }
}