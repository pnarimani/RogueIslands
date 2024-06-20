using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using RogueIslands.Boosters;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterResetVisualizer : BoosterActionVisualizer<BoosterResetAction>
    {
        [SerializeField] private MMF_Player _resetFeedback;

        protected override async UniTask OnBeforeBoosterExecuted(GameState state, BoosterResetAction action, BoosterView booster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            if (_resetFeedback != null)
                _resetFeedback.PlayFeedbacks();

            booster.UpdateDescription();
        }
    }
}