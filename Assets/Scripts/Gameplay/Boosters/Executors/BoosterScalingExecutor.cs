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

            if (action.AdditionChange is { } p)
            {
                PlayScalingAnimation(view, booster, p);
                scoringAction.Addition += p;
            }

            if (action.MultiplierChange is { } q)
            {
                PlayScalingAnimation(view, booster, q);
                scoringAction.Multiplier += q;
            }

            action.HasTriggered = true;

            if (!action.OneTime)
            {
                action.Progress = 0;
            }
        }

        private static void PlayScalingAnimation(IGameView view, IBooster booster, double change)
        {
            if (change > 0)
                view.GetBooster(booster.Id).GetScalingVisualizer().PlayScaleUp();
            else
                view.GetBooster(booster.Id).GetScalingVisualizer().PlayScaleDown();
        }
    }
}