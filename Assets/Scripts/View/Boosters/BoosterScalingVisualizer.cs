using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterScalingVisualizer : BoosterActionVisualizer<BoosterScalingAction>
    {
        [SerializeField] private MMF_Player _scaleUpFeedback;

        protected override async UniTask OnBeforeBoosterExecuted(GameState state, BoosterScalingAction action, BoosterView booster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            if (_scaleUpFeedback != null)
                _scaleUpFeedback.PlayFeedbacks();
            
            booster.UpdateDescription();
        }
    }
}