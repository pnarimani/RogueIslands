using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ModifyBuildingRangeExecutor : GameActionExecutor<ModifyBuildingRangeAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            ModifyBuildingRangeAction action)
        {
            foreach (var building in action.Source.Get(state, booster))
            {
                building.Range *= action.RangeMultiplier;
            }
        }
    }
}