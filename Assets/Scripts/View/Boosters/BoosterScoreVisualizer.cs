using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using RogueIslands.Boosters;
using TMPro;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterScoreVisualizer : BoosterActionVisualizer<ScoringAction>
    {
        [SerializeField] private MMF_Player _productScoreFeedback, _multScoreFeedback;
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
            AnimationScheduler.AllocateTime(0.4f);
            await UniTask.WaitForSeconds(wait);
            
            if (IsProduct(action))
            {
                GameUI.Instance.ProductBoosted(productBoost);
                
                _productAmountText.text = $"+{productBoost:F1}";
                
                if (_productScoreFeedback != null) 
                    _productScoreFeedback.PlayFeedbacks();
            }
            
            if (IsMult(action))
            {
                GameUI.Instance.MultBoosted(finalMult);

                if (action.XMult > 1)
                {
                    var xMultAmount = multBoost / action.XMult;
                    _multAmountText.text = $"x{xMultAmount:F1}";
                }
                else
                {
                    _multAmountText.text = $"+{multBoost:F1}";
                }
                
                if (_multScoreFeedback != null) 
                    _multScoreFeedback.PlayFeedbacks();
            }
        }
        
        private static bool IsProduct(ScoringAction action)
            => action.Products > 0;

        private static bool IsMult(ScoringAction action) 
            => action.PlusMult > 0 || action.XMult > 1;
    }
}