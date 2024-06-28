using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
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