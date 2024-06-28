using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
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