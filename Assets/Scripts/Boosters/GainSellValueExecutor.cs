namespace RogueIslands.Boosters
{
    public class GainSellValueExecutor : GameActionExecutor<GainSellValueAction>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster, GainSellValueAction action) 
            => booster.SellPrice += action.Amount;
    }
}