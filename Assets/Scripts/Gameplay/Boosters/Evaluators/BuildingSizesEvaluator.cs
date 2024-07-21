using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class BuildingSizesEvaluator : GameConditionEvaluator<BuildingSizeCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingSizeCondition condition)
        {
            if (state.CurrentEvent is not BuildingEvent { Building: { } building })
                return false;

            var inAllowed = condition.Allowed == null || condition.Allowed.Contains(building.Size);
            var notInBanned = condition.Banned == null || !condition.Banned.Contains(building.Size);
            
            return inAllowed && notInBanned;
        }
    }
}