using RogueIslands.Gameplay.Boosters.Conditions;
using static RogueIslands.Gameplay.Boosters.Conditions.TimeCondition;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class TimeConditionEvaluator : GameConditionEvaluator<TimeCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, TimeCondition condition)
        {
            return condition.TimeMode switch
            {
                Mode.Round => condition.FromStart
                    ? state.Round == condition.Time
                    : state.Round == GameState.RoundsPerAct - condition.Time,
                Mode.Act => condition.FromStart
                    ? state.Act == condition.Time
                    : state.Act == GameState.TotalActs - condition.Time,
                _ => false,
            };
        }
    }
}