using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class SelectedCardNotPlayedCondition : IGameCondition
    {
        public bool Evaluate(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var selectedCard = booster.GetEventAction<SelectCardAction>().SelectedCard;
            return state.BuildingsInHand.Contains(selectedCard);
        }
    }
}