using RogueIslands.Boosters.Actions;
using RogueIslands.Boosters.Conditions;

namespace RogueIslands.Boosters.Executors
{
    public class ModifyProbabilitiesExecutor : ModifyConditionActionExecutor<ProbabilityCondition, ModifyProbabilitiesAction>
    {
        protected override void ModifyCondition(ProbabilityCondition condition)
        {
            condition.FavorableOutcome *= 2;
        }
    }
}