using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using RogueIslands.Boosters;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace RogueIslands.View
{
    public class BoosterView : MonoBehaviour, IBoosterView
    {
        [SerializeField] private TextMeshProUGUI _name, _desc;
        [SerializeField] private MMF_Player _triggerFeedback;

        public Booster Data { get; private set; }

        public void Show(Booster booster)
        {
            Assert.IsNotNull(booster);

            Data = booster;
            _name.text = booster.Name;
            _desc.text = booster.Description;
        }

        public async void OnActionExecuted(GameState state, GameAction action)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(Mathf.Max(0.4f, _triggerFeedback.TotalDuration));
            AnimationScheduler.EnsureExtraTime(1.3f);

            double productBoost = 0, multBoost = 0;

            if (action is ScoringAction score)
            {
                if (score.Products > 0)
                {
                    productBoost = score.Products;
                }

                if (score.PlusMult > 0)
                {
                    multBoost = score.PlusMult;
                }

                if (score.XMult > 1)
                {
                    multBoost = state.ScoringState.Multiplier * score.XMult - state.ScoringState.Multiplier;
                }
            }

            await UniTask.WaitForSeconds(wait);

            if (productBoost > 0)
                GameUI.Instance.ProductBoosted(productBoost);
            
            if (multBoost > 0)
                GameUI.Instance.MultBoosted(multBoost);
            
            if (_triggerFeedback != null)
                _triggerFeedback.PlayFeedbacks();
        }
    }
}