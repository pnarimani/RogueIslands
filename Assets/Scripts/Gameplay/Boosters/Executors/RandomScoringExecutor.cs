using RogueIslands.Gameplay.Boosters.Actions;
using Unity.Mathematics;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class RandomScoringExecutor : GameActionExecutor<RandomScoringAction>
    {
        private Random _random;

        public RandomScoringExecutor(Random random)
        {
            _random = random;
        }

        protected override void Execute(GameState state, IGameView view, IBooster booster, RandomScoringAction action)
        {
            if (action.Products is { } products)
                state.ScoringState.Products += _random.NextDouble(0, products);
            if (action.PlusMult is { } plusMult)
                state.ScoringState.Multiplier += _random.NextDouble(0, plusMult);
            if (action.XMult is { } xMult)
                state.ScoringState.Multiplier *= _random.NextDouble(1, xMult);
        }
    }
}