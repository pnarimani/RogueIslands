using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterRetriggerVisualizer : MonoBehaviour
    {
        [SerializeField] private LabelFeedback _labelFeedback;
        
        public async UniTask Play()
        {
            await AnimationScheduler.ScheduleAndWait(1);
            await _labelFeedback.Play();
        }
    }
}