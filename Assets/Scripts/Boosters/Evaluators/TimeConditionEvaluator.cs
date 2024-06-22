using static RogueIslands.Boosters.TimeCondition;

namespace RogueIslands.Boosters
{
    public class TimeConditionEvaluator : GameConditionEvaluator<TimeCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, TimeCondition condition)
        {
            return condition.TimeMode switch
            {
                Mode.Day =>
                    condition.FromStart
                        ? state.Day == condition.Time
                        : state.Day == state.TotalDays - condition.Time,
                Mode.Week => condition.FromStart
                    ? state.Round == condition.Time
                    : state.Round == GameState.TotalRounds - condition.Time,
                Mode.Month => condition.FromStart
                    ? state.Act == condition.Time
                    : state.Act == GameState.TotalActs - condition.Time,
                Mode.TotalDays => state.TotalDays == condition.Time,
                _ => false,
            };
        }
    }
}