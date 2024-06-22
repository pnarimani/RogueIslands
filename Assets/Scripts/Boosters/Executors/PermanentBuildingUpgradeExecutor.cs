using RogueIslands.Boosters.Actions;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters.Executors
{
    public class PermanentBuildingUpgradeExecutor : GameActionExecutor<PermanentBuildingUpgradeAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            PermanentBuildingUpgradeAction action)
        {
            if (state.CurrentEvent is BuildingEvent e)
                e.Building.OutputUpgrade += action.ProductUpgrade;
        }
    }
}