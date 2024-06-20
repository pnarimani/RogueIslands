using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterRetriggerVisualizer : BoosterActionVisualizer
    {
        [SerializeField] private MMF_Player _retriggerFeedback;
        
        public override bool CanVisualize(GameAction action)
        {
            return action is RetriggerScoringBuildingAction;
        }

        public override async UniTask OnBeforeBoosterExecuted(GameState state, GameAction action, BoosterView booster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);
            
            if (_retriggerFeedback != null)
                _retriggerFeedback.PlayFeedbacks();
            
        }

        public override UniTask OnAfterBoosterExecuted(GameState state, GameAction action, BoosterView booster)
        {
         return UniTask.CompletedTask;
        }
    }
}