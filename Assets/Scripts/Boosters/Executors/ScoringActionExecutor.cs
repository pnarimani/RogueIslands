using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class ScoringActionExecutor : GameActionExecutor<ScoringAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, ScoringAction action)
        {
            if (action.Products is { } products)
                state.ScoringState.Products += products;
            if (action.PlusMult is { } plusMult)
                state.ScoringState.Multiplier += plusMult;
            if (action.XMult is { } xMult)
                state.ScoringState.Multiplier *= xMult;
        }
    }
}