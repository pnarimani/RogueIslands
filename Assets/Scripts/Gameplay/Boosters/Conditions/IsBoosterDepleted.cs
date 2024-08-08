using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class IsBoosterDepleted : IGameCondition
    {
        public bool Evaluate(IBooster booster)
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