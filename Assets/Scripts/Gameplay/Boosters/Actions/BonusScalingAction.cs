using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class BonusScalingAction : GameAction
    {
        public double? AdditionChange { get; set; }
        public double? MultiplierChange { get; set; }
        public double? ColorMultiplierChange { get; set; }
        public double? CategoryMultiplierChange { get; set; }
        public double? SizeMultiplierChange { get; set; }
            
        public bool OneTime { get; set; }
        public bool HasTriggered { get; set; }
        public int? Delay { get; set; }
        public int Progress { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var view = StaticResolver.Resolve<IGameView>();
            var scoringAction = booster.GetEventAction<BonusAction>();
            if (Delay is { } delay)
            {
                if (Progress < delay)
                {
                    Progress++;

                    if (Progress < delay)
                        return false;
                }
            }

            if (HasTriggered && OneTime)
                return false;

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
            
            if (ColorMultiplierChange is { } r)
            {
                PlayScalingAnimation(view, booster, r);
                scoringAction.ColorMultiplier += r;
            }
            
            if (CategoryMultiplierChange is { } s)
            {
                PlayScalingAnimation(view, booster, s);
                scoringAction.CategoryMultiplier += s;
            }
            
            if (SizeMultiplierChange is { } t)
            {
                PlayScalingAnimation(view, booster, t);
                scoringAction.SizeMultiplier += t;
            }

            HasTriggered = true;

            if (!OneTime)
            {
                Progress = 0;
            }

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