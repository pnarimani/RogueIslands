namespace RogueIslands.Boosters
{
    public class SelectedBuildingRangeConditionEvaluator : ConditionEvaluator<SelectedBuildingRangeCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedBuildingRangeCondition condition)
        {
            if(booster is not WorldBooster worldBooster)
                return false;
            
            var selectedBuilding = state.ScoringState.SelectedBuilding;
            if(selectedBuilding is null)
                return false;

            var sqrDist = (selectedBuilding.Position - worldBooster.Position).sqrMagnitude;
            return sqrDist <= worldBooster.Range * worldBooster.Range;
        }
    }
}