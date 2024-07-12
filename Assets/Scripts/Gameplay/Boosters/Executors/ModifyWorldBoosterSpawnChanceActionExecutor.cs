using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ModifyWorldBoosterSpawnChanceActionExecutor : GameActionExecutor<ModifyWorldBoosterSpawnAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            ModifyWorldBoosterSpawnAction action)
        {
            state.WorldBoosters.SpawnDistribution.Factor *= action.FactorMultiplier;
            state.WorldBoosters.SpawnCount *= action.CountMultiplier;
        }
    }
}