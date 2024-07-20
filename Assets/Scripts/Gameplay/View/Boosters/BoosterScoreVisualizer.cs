using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterScoreVisualizer : MonoBehaviour, IBoosterScoreVisualizer
    {
        [SerializeField] private LabelFeedback _productLabelFeedback, _multiLabelFeedback;
        [SerializeField] private CardTriggerFeedback _triggerFeedback;
        
        private double _previousScore;
        
        public async void MultiplierApplied(double multiplier, double products)
        {
            await AnimationScheduler.ScheduleAndWait(1f);
            
            _triggerFeedback.Play().Forget();
            GameUI.Instance.ProductBoosted(products);
            _multiLabelFeedback.SetText($"x{multiplier:F1}");
            await _multiLabelFeedback.Play();
        }

        public async void ProductApplied(double products)
        {
            await AnimationScheduler.ScheduleAndWait(1f);
            
            _triggerFeedback.Play().Forget();
            GameUI.Instance.ProductBoosted(products);
            _productLabelFeedback.SetText($"+{products:F1}");
            await _productLabelFeedback.Play();
        }
    }
}