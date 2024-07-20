using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterResetVisualizer : MonoBehaviour
    {
        [SerializeField] private LabelFeedback _labelFeedback;
        [SerializeField] private CardTriggerFeedback _triggerFeedback;

        public async UniTask Play()
        {
            await AnimationScheduler.ScheduleAndWait(1);
            _triggerFeedback.Play().Forget();
            await _labelFeedback.Play();
        }
    }
}