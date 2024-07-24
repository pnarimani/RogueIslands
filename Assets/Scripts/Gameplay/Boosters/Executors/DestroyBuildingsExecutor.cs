using RogueIslands.Gameplay.Boosters.Actions;
using UnityEngine;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class DestroyBuildingsExecutor : GameActionExecutor<DestroyBuildingsAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            DestroyBuildingsAction action)
        {
        }
    }
}