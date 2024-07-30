using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class IsBoosterDepletedEvaluator : GameConditionEvaluator<IsBoosterDepleted>
    {
        protected override bool Evaluate(GameState state, IBooster booster, IsBoosterDepleted condition)
        {
            var scoringAction = booster.GetEventAction<ScoringAction>();
            
            if (scoringAction.Multiplier is { } mult)
                return mult == 0;

            if (scoringAction.Addition is { } prod)
                return prod == 0;

            return false;
        }
    }
}