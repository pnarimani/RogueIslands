using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class GainSellValueExecutor : GameActionExecutor<GainSellValueAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, GainSellValueAction action)
        {
            if (booster is IPurchasableItem purchasable)
                purchasable.SellPrice += action.Amount;
        }
    }
}