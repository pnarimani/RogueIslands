using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class RetriggerBuildingExecutor : GameActionExecutor<RetriggerBuildingAction>
    {
        private readonly ScoringController _scoringController;

        public RetriggerBuildingExecutor(ScoringController scoringController)
        {
            _scoringController = scoringController;
        }
        
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            RetriggerBuildingAction action)
        {
            if (state.CurrentEvent is BuildingEvent e)
            {
                if(action.RemainingTriggers <= 0)
                    return;
                action.RemainingTriggers--;
                
                view.GetBooster(booster).GetRetriggerVisualizer().PlayRetrigger();
                _scoringController.TriggerBuilding(e.Building);
            }

            if (state.CurrentEvent is ResetRetriggers)
            {
                action.RemainingTriggers = action.RetriggerTimes;
            }
        }
    }
}