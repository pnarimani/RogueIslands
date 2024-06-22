using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using RogueIslands.Boosters;
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

        protected override async UniTask OnAfterBoosterExecuted(GameState state, ScoringAction action, BoosterView booster)
        {
            var productBoost = state.ScoringState.Products - _previousProduct;
            var finalMult = state.ScoringState.Multiplier;
            var multBoost = finalMult - _previousMult;
            
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

                if (action.XMult > 1)
                {
                    var xMultAmount = multBoost / action.XMult;
                    if (_multAmountText != null) 
                        _multAmountText.text = $"x{xMultAmount:F1}";
                }
                else
                {
                    if (_multAmountText != null) 
                        _multAmountText.text = $"+{multBoost:F1}";
                }

                await _multiLabelFeedback.Play();
            }
        }
        
        private static bool IsProduct(ScoringAction action)
            => action.Products > 0;

        private static bool IsMult(ScoringAction action) 
            => action.PlusMult > 0 || action.XMult > 1;

        [Button]
        public async void PlayFeedbacks()
        {
            await _multiLabelFeedback.Play();
        }
    }
}