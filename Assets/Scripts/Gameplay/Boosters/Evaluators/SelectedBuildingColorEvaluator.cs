using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class SelectedBuildingColorEvaluator : GameConditionEvaluator<BuildingColorCondition>
    {
        protected override bool Evaluate(GameState state, IBooster booster, BuildingColorCondition condition) 
            => state.CurrentEvent is BuildingEvent e && condition.Colors.Contains(e.Building.Color);
    }
}