using Cysharp.Threading.Tasks;
using RogueIslands.DependencyInjection;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.View.Audio;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterRetriggerVisualizer : MonoBehaviour, IBoosterRetriggerVisualizer
    {
        [SerializeField] private LabelFeedback _labelFeedback;
        [SerializeField] private CardTriggerFeedback _triggerFeedback;

        private LabelFeedback _dryRunLabel;

        private async UniTask Play()
        {
            await AnimationScheduler.ScheduleAndWait(1);
            StaticResolver.Resolve<IBoosterAudio>().BoosterTriggered();
            _triggerFeedback.Play().Forget();
            await _labelFeedback.Play();
        }

        public void PlayRetrigger()
        {
            Play().Forget();
        }

        public void ShowDryRunRetriggers(int count)
        {
            _dryRunLabel = Instantiate(_labelFeedback, _labelFeedback.transform.parent, true);
            _dryRunLabel.transform.localScale = Vector3.one * 0.75f;
            _dryRunLabel.SetText(count > 1 ? $"Again!<size=65%>x{count}" : $"Again!");
            _dryRunLabel.Show();
        }

        public void ShowDryRunProbability()
        {
            _dryRunLabel = Instantiate(_labelFeedback, _labelFeedback.transform.parent, true);
            _dryRunLabel.transform.localScale = Vector3.one * 0.75f;
            _dryRunLabel.SetText("???");
            _dryRunLabel.Show();
        }

        public void HideDryRun()
        {
            if (_dryRunLabel != null)
            {
                Destroy(_dryRunLabel.gameObject);
            }
        }
    }
}