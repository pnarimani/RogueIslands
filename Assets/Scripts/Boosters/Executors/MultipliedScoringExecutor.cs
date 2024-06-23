using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class MultipliedScoringExecutor : GameActionExecutor<MultipliedScoringAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            MultipliedScoringAction action)
        {
            int multiplier;
            if (action.MultiplyByDay)
                multiplier = state.TotalDays - state.Day;
            else if (action.MultiplyByIslandCount)
                multiplier = state.Clusters.Count;
            else
                multiplier = 1;
            
            if (action.Products is { } products)
                state.ScoringState.Products += products * multiplier;
            if (action.PlusMult is { } plusMult)
                state.ScoringState.Multiplier += plusMult * multiplier;
            if (action.XMult is { } xMult)
                state.ScoringState.Multiplier *= xMult * multiplier;
        }
    }
}