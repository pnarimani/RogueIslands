using System;
using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterScoreVisualizer : BoosterActionVisualizer<ScoringAction>, IBoosterScoreVisualizer
    {
        [SerializeField] private LabelFeedback _productLabelFeedback, _multiLabelFeedback;

        private double _previousScore;

        protected override UniTask OnBeforeBoosterExecuted(GameState state, ScoringAction action, BoosterView booster)
        {
            throw new NotImplementedException();
        }

        protected override async UniTask OnAfterBoosterExecuted(GameState state, ScoringAction action,
            BoosterView booster)
        {
            throw new NotImplementedException();
        }

        public async void MultiplierApplied(double multiplier, double products)
        {
            await AnimationScheduler.ScheduleAndWait(1f);
            
            GameUI.Instance.ProductBoosted(products);
            _multiLabelFeedback.SetText($"x{multiplier:F1}");
            await _multiLabelFeedback.Play();
        }

        public async void ProductApplied(double products)
        {
            await AnimationScheduler.ScheduleAndWait(1f);
            
            GameUI.Instance.ProductBoosted(products);
            _productLabelFeedback.SetText($"+{products:F1}");
            await _productLabelFeedback.Play();
        }
    }
}