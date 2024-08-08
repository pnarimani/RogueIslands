using System.Collections.Generic;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class CategoryPlayCountSource : ISource<int>
    {
        public Category Category { get; set; }

        public IEnumerable<int> Get(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            state.Metadata.RunCategoryPlayCount.TryGetValue(Category, out var value);
            return new[] { value };
        }
    }
}