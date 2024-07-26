using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterResetVisualizer : MonoBehaviour, IBoosterResetVisualizer
    {
        [SerializeField] private LabelFeedback _labelFeedback;
        [SerializeField] private CardTriggerFeedback _triggerFeedback;
        private LabelFeedback _dryRunLabel;

        public async UniTask Play()
        {
            await AnimationScheduler.ScheduleAndWait(1);
            _triggerFeedback.Play().Forget();
            await _labelFeedback.Play();
        }

        public void PlayReset()
        {
            Play().Forget();
        }

        public void HideDryRun()
        {
            if (_dryRunLabel != null)
            {
                Destroy(_dryRunLabel.gameObject);
            }
        }

        public void ShowDryRunReset()
        {
            _dryRunLabel = Instantiate(_labelFeedback, _labelFeedback.transform.parent, true);
            _dryRunLabel.Show();
        }

        public void ShowDryRunProbability()
        {
            _dryRunLabel = Instantiate(_labelFeedback, _labelFeedback.transform.parent, true);
            _dryRunLabel.SetText("???");
            _dryRunLabel.Show();
        }
    }
}