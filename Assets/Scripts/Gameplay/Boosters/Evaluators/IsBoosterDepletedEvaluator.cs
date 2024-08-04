using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class IsBoosterDepletedEvaluator : GameConditionEvaluator<IsBoosterDepleted>
    {
        protected override bool Evaluate(GameState state, IBooster booster, IsBoosterDepleted condition)
        {
            var scoringAction = booster.GetEventAction<ScoringAction>();

            if (scoringAction is { Multiplier: { } mult })
                return mult == 0;

            if (scoringAction is { Addition: { } prod })
                return prod == 0;

            var retrigger = booster.GetEventAction<RetriggerBuildingAction>();
            if (retrigger is { RemainingCharges: { } charges })
                return charges == 0;

            return false;
        }
    }
}