using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ScoreScalingAction : GameAction
    {
        public double? AdditionChange { get; set; }
        public double? MultiplierChange { get; set; }

        public bool OneTime { get; set; }
        public bool HasTriggered { get; set; }
        public int? Delay { get; set; }
        public int Progress { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var scoringAction = booster.GetEventAction<ScoringAction>();
            if (Delay is { } delay)
                if (Progress < delay)
                {
                    Progress++;

                    if (Progress < delay)
                        return false;
                }

            if (HasTriggered && OneTime)
                return false;

            var view = StaticResolver.Resolve<IGameView>();

            if (AdditionChange is { } p)
            {
                PlayScalingAnimation(view, booster, p);
                scoringAction.Addition += p;
            }

            if (MultiplierChange is { } q)
            {
                PlayScalingAnimation(view, booster, q);
                scoringAction.Multiplier += q;
            }

            HasTriggered = true;

            if (!OneTime)
                Progress = 0;

            return true;
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