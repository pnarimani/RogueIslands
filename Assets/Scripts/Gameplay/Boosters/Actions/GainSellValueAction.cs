using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{   
    public class GainSellValueAction : GameAction
    {
        public int Amount { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var view = StaticResolver.Resolve<IGameView>();
            if (booster is IPurchasableItem purchasable)
            {
                purchasable.SellPrice += Amount;
                view.GetBooster(booster.Id).GetScalingVisualizer().PlayScaleUp();
                return true;
            }

            return false;
        }
    }
}