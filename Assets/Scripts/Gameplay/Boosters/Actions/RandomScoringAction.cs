using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class RandomScoringAction : ScoringAction
    {
        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var view = StaticResolver.Resolve<IGameView>();
            
            var rand = state.GetRandomForType<RandomScoringAction>().ForAct(state.Act);

            if (Addition is { } products)
                state.TransientScore += rand.NextDouble(0, products);

            if (Multiplier is { } xMult)
            {
                var randomMult = rand.NextDouble(1, xMult);
                var finalScore = state.TransientScore * randomMult;
                var diff = finalScore - state.TransientScore;
                state.TransientScore = finalScore;

                view.GetBooster(booster.Id).GetScoringVisualizer().MultiplierApplied(randomMult, diff);
            }

            return true;
        }
    }
}