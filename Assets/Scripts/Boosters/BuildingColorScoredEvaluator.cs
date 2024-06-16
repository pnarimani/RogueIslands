namespace RogueIslands.Boosters
{
    public class BuildingColorScoredEvaluator : ConditionEvaluator<BuildingColorScoredCondition>
    {
        protected override bool Evaluate(GameState state, BuildingColorScoredCondition condition) 
            => state.ScoringState.CurrentScoringBuilding is { } building && building.Color == condition.ColorTag;
    }
}