using System.Collections.Generic;
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

        private List<LabelFeedback> _dryRunLabels = new();

        public async void MultiplierApplied(double multiplier, double products)
        {
            await AnimationScheduler.ScheduleAndWait(1f);

            _triggerFeedback.Play().Forget();
            GameUI.Instance.ProductBoosted(products);
            _multiLabelFeedback.SetText($"x{multiplier:0.#}");
            await _multiLabelFeedback.Play();
        }

        public async void ProductApplied(double products)
        {
            await AnimationScheduler.ScheduleAndWait(1f);

            _triggerFeedback.Play().Forget();
            GameUI.Instance.ProductBoosted(products);
            _productLabelFeedback.SetText($"+{products:0.#}");
            await _productLabelFeedback.Play();
        }

        public void ShowDryRunMultiplier(Dictionary<double, int> multipliersAndCount)
        {
            foreach (var (mult, count) in multipliersAndCount)
            {
                var label = Instantiate(_multiLabelFeedback, _multiLabelFeedback.transform.parent, true);
                label.SetText(count > 1 ? $"x{mult:0.#} x {count}" : $"x{mult:0.#}");
                label.Show();
                _dryRunLabels.Add(label);
            }
        }

        public void ShowDryRunProducts(Dictionary<double, int> productsAndCount)
        {
            foreach (var (prod, count) in productsAndCount)
            {
                var label = Instantiate(_productLabelFeedback, _productLabelFeedback.transform.parent, true);
                label.SetText(count > 1 ? $"+{prod:0.#} x {count}" : $"+{prod:0.#}");
                label.Show();
                _dryRunLabels.Add(label);
            }
        }

        public void ShowDryRunAddProbability()
        {
            var label = Instantiate(_productLabelFeedback, _productLabelFeedback.transform.parent, true);
            label.SetText("???");
            label.Show();
            _dryRunLabels.Add(label);
        }

        public void ShowDryRunMultiplyProbability()
        {
            var label = Instantiate(_multiLabelFeedback, _multiLabelFeedback.transform.parent, true);
            label.SetText("???");
            label.Show();
            _dryRunLabels.Add(label);
        }

        public void HideDryRun()
        {
            foreach (var label in _dryRunLabels)
            {
                Destroy(label.gameObject);
            }

            _dryRunLabels.Clear();
        }
    }
}