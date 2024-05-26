namespace RogueIslands.Boosters
{
    public class MultiplierModifierExecutor : GameActionExecutor<MultiplierModifierAction>
    {
        protected override void Execute(GameState state, MultiplierModifierAction action)
        {
            state.ScoringState.Multiplier += action.PlusMult;
            state.ScoringState.Multiplier *= action.XMult;
        }
    }
}