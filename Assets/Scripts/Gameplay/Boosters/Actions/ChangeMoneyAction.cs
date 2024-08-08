using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Sources;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ChangeMoneyAction : GameAction
    {
        public bool IsImmediate { get; set; }
        public int Change { get; set; }
        public ISource<int> Multiplier { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var view = StaticResolver.Resolve<IGameView>();
            
            var mult = 1;
            if (Multiplier != null)
                mult = Multiplier.Get(booster).First();

            var finalChange = Change * mult;
            if (finalChange == 0)
                return false;
            
            if (IsImmediate)
            {
                state.Money += finalChange;
                view.GetBooster(booster.Id).GetMoneyVisualizer().Play(finalChange);
            }
            else
            {
                state.MoneyChanges.Add(new MoneyChange
                {
                    Reason = booster.Name,
                    Change = finalChange,
                });
            }

            return true;
        }
    }
}