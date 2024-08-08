using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Sources;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class MultipliedScoringAction : ScoringAction
    {
        public ISource<int> Factor { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var multiplier = Factor.Get(booster).First();

            if (multiplier <= 0)
                return false;

            var state = StaticResolver.Resolve<GameState>();
            var view = StaticResolver.Resolve<IGameView>();

            var boosterView = view.GetBooster(booster.Id);

            if (Addition is { } products)
            {
                state.TransientScore += products * multiplier;
                boosterView.GetScoringVisualizer().ProductApplied(products * multiplier);
            }

            if (Multiplier is { } xMult)
            {
                var multiplied = 1 + xMult * multiplier;
                var finalProducts = state.TransientScore * multiplied;
                var diff = finalProducts - state.TransientScore;
                state.TransientScore = finalProducts;
                boosterView.GetScoringVisualizer().MultiplierApplied(multiplied, diff);
            }

            return true;
        }
    }
}