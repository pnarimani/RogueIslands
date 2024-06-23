using Cysharp.Threading.Tasks;
using RogueIslands.Boosters.Actions;
using RogueIslands.View.Feedbacks;
using TMPro;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterScoreVisualizer : BoosterActionVisualizer<ScoringAction>
    {
        [SerializeField] private LabelFeedback _productLabelFeedback, _multiLabelFeedback;
        [SerializeField] private TextMeshProUGUI _productAmountText, _multAmountText;

        private double _previousProduct, _previousMult;

        protected override UniTask OnBeforeBoosterExecuted(GameState state, ScoringAction action, BoosterView booster)
        {
            if (IsProduct(action))
                _previousProduct = state.ScoringState.Products;

            if (IsMult(action))
                _previousMult = state.ScoringState.Multiplier;

            return UniTask.CompletedTask;
        }

        protected override async UniTask OnAfterBoosterExecuted(GameState state, ScoringAction action,
            BoosterView booster)
        {
            var productBoost = state.ScoringState.Products - _previousProduct;
            var finalMult = state.ScoringState.Multiplier;
            var dividedDelta = finalMult / _previousMult;
            var delta = finalMult - _previousMult;

            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            if (IsProduct(action))
            {
                GameUI.Instance.ProductBoosted(productBoost);

                if (_productAmountText != null)
                    _productAmountText.text = $"+{productBoost:F1}";

                await _productLabelFeedback.Play();
            }

            if (IsMult(action))
            {
                GameUI.Instance.MultBoosted(finalMult);

                if (action.XMult != null && _multAmountText != null) 
                    _multAmountText.text = $"x{dividedDelta:F1}";

                if (action.PlusMult != null && _multAmountText != null) 
                    _multAmountText.text = $"+{delta:F1}";

                await _multiLabelFeedback.Play();
            }
        }

        private static bool IsProduct(ScoringAction action)
            => action.Products != null;

        private static bool IsMult(ScoringAction action)
            => action.PlusMult != null || action.XMult != null;
    }
}