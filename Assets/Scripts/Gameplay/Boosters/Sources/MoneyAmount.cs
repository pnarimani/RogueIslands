using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class MoneyAmount : ISource<int>
    {
        public int DivideBy { get; set; } = 1;
        
        public IEnumerable<int> Get(GameState state, IBooster booster)
        {
            yield return state.Money / DivideBy;
        }
    }
}