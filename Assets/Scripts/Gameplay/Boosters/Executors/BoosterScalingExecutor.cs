using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class BoosterScalingExecutor : GameActionExecutor<BoosterScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, BoosterScalingAction action)
        {
            var scoringAction = booster.GetEventAction<ScoringAction>();
            if (action.Delay is { } delay)
            {
                if (action.Progress < delay)
                {
                    action.Progress++;

                    if (action.Progress < delay)
                        return;
                }
            }

            if (action.HasTriggered && action.OneTime)
                return;

            if (action.ProductChange is { } p)
            {
                PlayScalingAnimation(view, booster, p);
                scoringAction.Products += p;
            }

            if (action.MultiplierChange is { } q)
            {
                PlayScalingAnimation(view, booster, q);
                scoringAction.Multiplier += q;
            }

            action.HasTriggered = true;
        }

        private static void PlayScalingAnimation(IGameView view, IBooster booster, double change)
        {
            if (change > 0)
                view.GetBooster(booster).GetScalingVisualizer().PlayScaleUp();
            else
                view.GetBooster(booster).GetScalingVisualizer().PlayScaleDown();
        }
    }
}