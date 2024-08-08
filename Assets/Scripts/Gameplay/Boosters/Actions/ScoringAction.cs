using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ScoringAction : GameAction
    {
        public double? Multiplier { get; set; }
        public double? Addition { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var view = StaticResolver.Resolve<IGameView>();
            
            if (Addition is { } products and > 0)
            {
                state.TransientScore += products;
                view.GetBooster(booster.Id).GetScoringVisualizer().ProductApplied(products);
            }

            if (Multiplier is { } xMult)
            {
                var final = state.TransientScore * xMult;
                var diff = final - state.TransientScore;
                state.TransientScore = final;
                view.GetBooster(booster.Id).GetScoringVisualizer().MultiplierApplied(xMult, diff);
            }

            return true;
        }
    }
}