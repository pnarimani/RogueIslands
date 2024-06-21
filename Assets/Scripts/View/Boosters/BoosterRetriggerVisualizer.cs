using Cysharp.Threading.Tasks;
using RogueIslands.Boosters;
using RogueIslands.View.Feedbacks;
using UnityEngine;

namespace RogueIslands.View.Boosters
{
    public class BoosterRetriggerVisualizer : BoosterActionVisualizer
    {
        [SerializeField] private LabelFeedback _labelFeedback;

        public override bool CanVisualize(GameAction action)
        {
            return action is RetriggerScoringBuildingAction;
        }

        public override async UniTask OnBeforeBoosterExecuted(GameState state, GameAction action, BoosterView booster)
        {
            var wait = AnimationScheduler.GetAnimationTime();
            AnimationScheduler.AllocateTime(0.2f);
            await UniTask.WaitForSeconds(wait);

            await _labelFeedback.Play();
        }

        public override UniTask OnAfterBoosterExecuted(GameState state, GameAction action, BoosterView booster)
        {
            return UniTask.CompletedTask;
        }
    }
}