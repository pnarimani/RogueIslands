using Cysharp.Threading.Tasks;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View.Feedbacks;
using RogueIslands.View.Audio;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterScalingVisualizer : MonoBehaviour, IBoosterScalingVisualizer
    {
        [SerializeField] private LabelFeedback _labelFeedback, _scaleDownFeedback;
        [SerializeField] private CardTriggerFeedback _triggerFeedback;
        
        private LabelFeedback _dryRunLabel;

        public async void PlayScaleUp()
        {
            await AnimationScheduler.ScheduleAndWait(1, 0.3f);
            StaticResolver.Resolve<IBoosterAudio>().BoosterTriggered();
            _triggerFeedback.Play().Forget();
            await _labelFeedback.Play();
        }

        public async void PlayScaleDown()
        {
            await AnimationScheduler.ScheduleAndWait(1, 0.3f);
            StaticResolver.Resolve<IBoosterAudio>().BoosterTriggered();
            _triggerFeedback.Play().Forget();
            await _scaleDownFeedback.Play();
        }

        public void HideDryRun()
        {
            if (_dryRunLabel != null)
            {
                Destroy(_dryRunLabel.gameObject);
            }
        }

        public void ShowDryRunScaleUp(int count)
        {
            _dryRunLabel = Instantiate(_labelFeedback, _labelFeedback.transform.parent, true);
            _dryRunLabel.transform.localScale = Vector3.one * 0.75f;
            _dryRunLabel.Show();
        }

        public void ShowDryRunScaleDown(int count)
        {
            _dryRunLabel = Instantiate(_scaleDownFeedback, _scaleDownFeedback.transform.parent, true);
            _dryRunLabel.transform.localScale = Vector3.one * 0.75f;
            _dryRunLabel.Show();
        }

        public void ShowDryRunProbability()
        {
            _dryRunLabel = Instantiate(_labelFeedback, _labelFeedback.transform.parent, true);
            _dryRunLabel.transform.localScale = Vector3.one * 0.75f;
            _dryRunLabel.SetText("???");
            _dryRunLabel.Show();
        }
    }
}