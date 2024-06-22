namespace RogueIslands.Boosters
{
    public class MultipliedScoringExecutor : GameActionExecutor<MultipliedScoringAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            MultipliedScoringAction action)
        {
            if (action.MultiplyByDay)
            {
                var remDays = state.TotalDays - state.Day;
                state.ScoringState.Products += action.Products * remDays;
                state.ScoringState.Multiplier += action.PlusMult * remDays;
                state.ScoringState.Multiplier *= action.XMult * remDays;
            }
            else if (action.MultiplyByIslandCount)
            {
                var count = state.Clusters.Count;
                state.ScoringState.Products += action.Products * count;
                state.ScoringState.Multiplier += action.PlusMult * count;
                state.ScoringState.Multiplier *= action.XMult * count;
            }
            else
            {
                state.ScoringState.Products += action.Products;
                state.ScoringState.Multiplier += action.PlusMult;
                state.ScoringState.Multiplier *= action.XMult;
            }
        }
    }
}