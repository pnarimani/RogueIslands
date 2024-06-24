using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class ModifyWorldBoosterSpawnChanceActionExecutor : GameActionExecutor<ModifyWorldBoosterSpawnAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            ModifyWorldBoosterSpawnAction action)
        {
            state.WorldBoosters.SpawnChance *= action.SpawnMultiplier;
            state.WorldBoosters.Count *= action.CountMultiplier;
        }
    }
}