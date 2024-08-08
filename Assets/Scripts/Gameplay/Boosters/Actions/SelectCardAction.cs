using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class SelectCardAction : GameAction
    {
        public Building SelectedCard { get; set; }
        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var rand = state.GetRandomForType<SelectCardAction>().ForAct(state.Act);
            SelectedCard = state.BuildingsInHand.ToArray().SelectRandom(rand);
            return true;
        }
    }
}