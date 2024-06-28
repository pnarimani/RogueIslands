using System.Linq;
using Cysharp.Threading.Tasks;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Gameplay.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.Gameplay.View.Boosters
{
    public class BoosterScoreVisualizer : BoosterActionVisualizer<ScoringAction>
    {
        [SerializeField] private LabelFeedback _productLabelFeedback, _multiLabelFeedback;

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
            var productDelta = state.ScoringState.Products - _previousProduct;
            var finalMult = state.ScoringState.Multiplier;
            var dividedDelta = finalMult / _previousMult;
            var delta = finalMult - _previousMult;

            Building responsibleBuilding = null;

            if (state.CurrentEvent is BuildingRemainedInHand buildingEvent)
                responsibleBuilding = buildingEvent.Building;

            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            if (IsProduct(action))
            {
                GameUI.Instance.ProductBoosted(productDelta);

                if (responsibleBuilding != null)
                {
                    await GetBuildingView(responsibleBuilding).BuildingMadeProduct(productDelta);
                }
                else
                {
                    _productLabelFeedback.SetText($"+{productDelta:F1}");
                    await _productLabelFeedback.Play();
                }
            }

            if (IsMult(action))
            {
                GameUI.Instance.MultBoosted(finalMult);

                if (action.XMult != null)
                {
                    if (responsibleBuilding != null)
                        await GetBuildingView(responsibleBuilding).BuildingMadeXMult(dividedDelta);
                    else
                        _multiLabelFeedback.SetText($"x{dividedDelta:F1}");
                }

                if (action.PlusMult != null)
                {
                    if (responsibleBuilding != null)
                        await GetBuildingView(responsibleBuilding).BuildingMadePlusMult(delta);
                    else
                        _multiLabelFeedback.SetText($"+{delta:F1}");
                }

                if (responsibleBuilding == null)
                    await _multiLabelFeedback.Play();
            }
        }

        private static BuildingCardView GetBuildingView(Building responsibleBuilding)
        {
            return ObjectRegistry.GetBuildingCards()
                .First(c => c.Data == responsibleBuilding);
        }

        private static bool IsProduct(ScoringAction action)
            => action.Products != null;

        private static bool IsMult(ScoringAction action)
            => action.PlusMult != null || action.XMult != null;
    }
}