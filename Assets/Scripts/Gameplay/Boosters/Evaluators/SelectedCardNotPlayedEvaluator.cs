using System.Linq;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class SelectedCardNotPlayedEvaluator : GameConditionEvaluator<SelectedCardNotPlayedCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedCardNotPlayedCondition condition)
        {
            var selectedCard = booster.GetEventAction<SelectCardAction>().SelectedCard;
            return state.BuildingsInHand.Contains(selectedCard);
        }
    }
}