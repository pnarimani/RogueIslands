using System.Linq;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class SelectedBuildingColorEvaluator : GameConditionEvaluator<BuildingColorCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingColorCondition condition) 
            => state.CurrentEvent is BuildingEvent e && condition.Colors.Contains(e.Building.Color);
    }
}