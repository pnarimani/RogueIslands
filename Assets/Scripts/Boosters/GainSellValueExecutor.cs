namespace RogueIslands.Boosters
{
    public class GainSellValueExecutor : GameActionExecutor<GainSellValueAction>
    {
        protected override void Execute(GameState state, Booster booster, GainSellValueAction action) 
            => booster.SellPrice += action.Amount;
    }
}