namespace RogueIslands.Boosters
{
    public class ScoringActionExecutor : GameActionExecutor<ScoringAction>
    {
        protected override void Execute(GameState state, Booster booster, ScoringAction action)
        {
            state.ScoringState.Products += action.Products;
            state.ScoringState.Multiplier += action.PlusMult;
            state.ScoringState.Multiplier *= action.XMult;
        }
    }
}