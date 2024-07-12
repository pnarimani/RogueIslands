using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class RandomScoringExecutor : GameActionExecutor<RandomScoringAction>
    {
        private RogueRandom _random = new(1);

        protected override void Execute(GameState state, IGameView view, IBooster booster, RandomScoringAction action)
        {
            if (action.Products is { } products)
                state.ScoringState.Products += _random.ForAct(state.Act).NextDouble(0, products);
            if (action.PlusMult is { } plusMult)
                state.ScoringState.Multiplier += _random.ForAct(state.Act).NextDouble(0, plusMult);
            if (action.XMult is { } xMult)
                state.ScoringState.Multiplier *= _random.ForAct(state.Act).NextDouble(1, xMult);
        }
    }
}