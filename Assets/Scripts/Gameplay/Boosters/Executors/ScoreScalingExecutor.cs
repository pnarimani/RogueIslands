using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ScoreScalingExecutor : GameActionExecutor<ScoreScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, ScoreScalingAction action)
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
    
    public class BonusScalingExecutor : GameActionExecutor<BonusScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, BonusScalingAction action)
        {
            var scoringAction = booster.GetEventAction<BonusAction>();
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
            
            if (action.ColorMultiplierChange is { } r)
            {
                PlayScalingAnimation(view, booster, r);
                scoringAction.ColorMultiplier += r;
            }
            
            if (action.CategoryMultiplierChange is { } s)
            {
                PlayScalingAnimation(view, booster, s);
                scoringAction.CategoryMultiplier += s;
            }
            
            if (action.SizeMultiplierChange is { } t)
            {
                PlayScalingAnimation(view, booster, t);
                scoringAction.SizeMultiplier += t;
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