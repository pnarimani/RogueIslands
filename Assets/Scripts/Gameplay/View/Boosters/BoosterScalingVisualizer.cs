using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterScalingVisualizer : MonoBehaviour, IBoosterScalingVisualizer
    {
        [SerializeField] private LabelFeedback _labelFeedback, _scaleDownFeedback;
        [SerializeField] private CardTriggerFeedback _triggerFeedback;

        public async void PlayScaleUp()
        {
            await AnimationScheduler.ScheduleAndWait(1, 0.3f);
            _triggerFeedback.Play().Forget();
            await _labelFeedback.Play();
        }

        public async void PlayScaleDown()
        {
            await AnimationScheduler.ScheduleAndWait(1, 0.3f);
            _triggerFeedback.Play().Forget();
            await _scaleDownFeedback.Play();
        }
    }
}