namespace RogueIslands.Boosters
{
    public class TimeConditionEvaluator : ConditionEvaluator<TimeCondition>
    {
        protected override bool Evaluate(GameState state, TimeCondition condition)
        {
            return condition.TimeMode switch
            {
                TimeCondition.Mode.Day =>
                    condition.FromStart
                        ? state.Day == condition.Time
                        : state.Day == state.TotalDays - condition.Time,
                TimeCondition.Mode.Week => condition.FromStart
                    ? state.Week == condition.Time
                    : state.Week == GameState.TotalWeeks - condition.Time,
                TimeCondition.Mode.Month => condition.FromStart
                    ? state.Month == condition.Time
                    : state.Month == state.TotalMonths - condition.Time,
                _ => false,
            };
        }
    }
}