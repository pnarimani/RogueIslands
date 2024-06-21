using System.Linq;

namespace RogueIslands.Boosters
{
    public class BadEyesConditionEvaluator : ConditionEvaluator<SelectedBuildingColorCondition>,
        IEvaluationConditionOverride
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedBuildingColorCondition condition)
        {
            if (state.ScoringState is not { SelectedBuilding: not null })
                return false;

            var conditionColors = condition.Colors.ToList();
            if (conditionColors.Contains(ColorTag.Blue))
                conditionColors.Add(ColorTag.Red);
            if (conditionColors.Contains(ColorTag.Black))
                conditionColors.Add(ColorTag.White);
            return conditionColors.Contains(state.ScoringState.SelectedBuilding.Color);
        }
    }
}