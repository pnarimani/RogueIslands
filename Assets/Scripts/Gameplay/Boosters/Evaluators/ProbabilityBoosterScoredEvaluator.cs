using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class ProbabilityBoosterScoredEvaluator : GameConditionEvaluator<ProbabilityBoosterScoredCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, ProbabilityBoosterScoredCondition condition)
        {
            if (state.CurrentEvent is not BoosterScoredEvent boosterScored)
                return false;

            if (boosterScored.Booster.EventAction is null)
                return false;
            
            return boosterScored.Booster.EventAction.GetAllConditions().Any(c => c is ProbabilityCondition);
        }
    }
}