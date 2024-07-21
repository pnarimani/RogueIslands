using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Sources;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class BuildingSizeEvaluator : GameConditionEvaluator<BuildingSizeCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingSizeCondition condition)
        {
            condition.Source ??= new BuildingFromCurrentEvent();
            
            foreach (var building in condition.Source.Get(state, booster))
            {
                var inAllowed = condition.Allowed == null || condition.Allowed.Contains(building.Size);
                var notInBanned = condition.Banned == null || !condition.Banned.Contains(building.Size);

                if (!(inAllowed && notInBanned))
                    return false;
            }

            return true;
        }
    }
}