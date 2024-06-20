using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public abstract class BoosterActionVisualizer : MonoBehaviour
    {
        public abstract bool CanVisualize(GameAction action);
        public abstract UniTask OnBeforeBoosterExecuted(GameState state, GameAction action, BoosterView booster);
        public abstract UniTask OnAfterBoosterExecuted(GameState state, GameAction action, BoosterView booster);
    }

    public abstract class BoosterActionVisualizer<T> : BoosterActionVisualizer where T : GameAction
    {
        public sealed override bool CanVisualize(GameAction action) => action is T;

        public sealed override UniTask OnBeforeBoosterExecuted(GameState state, GameAction action, BoosterView booster) 
            => OnBeforeBoosterExecuted(state, (T)action, booster);

        public sealed override UniTask OnAfterBoosterExecuted(GameState state, GameAction action, BoosterView booster)
            => OnAfterBoosterExecuted(state, (T)action, booster);

        protected virtual UniTask OnBeforeBoosterExecuted(GameState state, T action, BoosterView booster) 
            => UniTask.CompletedTask;
        
        protected virtual UniTask OnAfterBoosterExecuted(GameState state, T action, BoosterView booster) 
            => UniTask.CompletedTask;
    }
}