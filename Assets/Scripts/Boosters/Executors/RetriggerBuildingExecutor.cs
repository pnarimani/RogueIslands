using RogueIslands.Boosters.Actions;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters.Executors
{
    public class RetriggerBuildingExecutor : GameActionExecutor<RetriggerBuildingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            RetriggerBuildingAction action)
        {
            if (state.CurrentEvent is BuildingEvent e)
                e.Building.RemainingTriggers += action.RetriggerTimes;
        }
    }
}