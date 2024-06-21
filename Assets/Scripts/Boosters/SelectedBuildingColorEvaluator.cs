using System.Linq;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingColorEvaluator : ConditionEvaluator<SelectedBuildingColorCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, SelectedBuildingColorCondition condition) 
            => state.CurrentEvent is BuildingEvent e && condition.Colors.Contains(e.Building.Color);
    }
}