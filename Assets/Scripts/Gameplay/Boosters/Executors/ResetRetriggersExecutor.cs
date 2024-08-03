using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ResetRetriggersExecutor : GameActionExecutor<ResetRetriggersAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, ResetRetriggersAction action)
        {
            var retrigger = booster.GetEventAction<RetriggerBuildingAction>();
            retrigger.RemainingTriggers = retrigger.RetriggerTimes;
        }
    }
}