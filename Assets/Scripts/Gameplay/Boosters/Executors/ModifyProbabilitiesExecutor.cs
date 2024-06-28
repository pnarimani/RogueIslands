using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ModifyProbabilitiesExecutor : ModifyConditionActionExecutor<ProbabilityCondition, ModifyProbabilitiesAction>
    {
        protected override void ModifyCondition(ProbabilityCondition condition)
        {
            condition.FavorableOutcome *= 2;
        }
    }
}