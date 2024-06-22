using Cysharp.Threading.Tasks;
using RogueIslands.Boosters;
using RogueIslands.Boosters.Actions;
using RogueIslands.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterScalingVisualizer : BoosterActionVisualizer<BoosterScalingAction>
    {
        [SerializeField] private LabelFeedback _labelFeedback;
        

        protected override async UniTask OnBeforeBoosterExecuted(GameState state, BoosterScalingAction action, BoosterView booster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            await _labelFeedback.Play();
        }
    }
}