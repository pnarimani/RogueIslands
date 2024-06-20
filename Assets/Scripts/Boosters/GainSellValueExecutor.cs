namespace RogueIslands.Boosters
{
    public class GainSellValueExecutor : GameActionExecutor<GainSellValueAction>
    {
        protected override void Execute(GameState state, IGameView view, BoosterCard booster, GainSellValueAction action) 
            => booster.SellPrice += action.Amount;
    }
}