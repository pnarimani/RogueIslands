using Cysharp.Threading.Tasks;
using RogueIslands.Boosters;
using RogueIslands.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterResetVisualizer : BoosterActionVisualizer<BoosterResetAction>
    {
        [SerializeField] private LabelFeedback _labelFeedback;
        
        protected override async UniTask OnBeforeBoosterExecuted(GameState state, BoosterResetAction action, BoosterView booster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            await _labelFeedback.Play();

            booster.UpdateDescription();
        }
    }
}