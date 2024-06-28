using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Executors
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