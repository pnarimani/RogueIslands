using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterScalingVisualizer : BoosterActionVisualizer<BoosterScalingAction>
    {
        [SerializeField] private LabelFeedback _labelFeedback;
        

        protected override async UniTask OnBeforeBoosterExecuted(GameState state, BoosterScalingAction action, BoosterView booster)
        {
            await AnimationScheduler.ScheduleAndWait(0.4f, 0.3f);

            await _labelFeedback.Play();
        }
    }
}