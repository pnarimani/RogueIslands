using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Sources;
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
            if(action.RemainingCharges is <= 0)
                return;
            
            if (state.CurrentEvent is ResetTriggersEvent)
            {
                action.RemainingTriggers = action.RetriggerTimes;
            }
            else
            {
                if (action.RemainingTriggers <= 0)
                {
                    if (action.RemainingCharges != null)
                        action.RemainingCharges--;
                    return;
                }
                action.RemainingTriggers--;

                action.Buildings ??= new BuildingFromCurrentEvent();

                view.GetBooster(booster.Id).GetRetriggerVisualizer().PlayRetrigger();

                foreach (var building in action.Buildings.Get(state, booster))
                {
                    _scoringController.TriggerBuilding(building);
                }
            }
        }
    }
}