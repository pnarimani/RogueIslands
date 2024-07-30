using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class RandomScoringExecutor : GameActionExecutor<RandomScoringAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, RandomScoringAction action)
        {
            var rand = state.GetRandomForType<RandomScoringAction>().ForAct(state.Act);

            if (action.Addition is { } products)
                state.TransientScore += rand.NextDouble(0, products);

            if (action.Multiplier is { } xMult)
            {
                var randomMult = rand.NextDouble(1, xMult);
                var finalScore = state.TransientScore * randomMult;
                var diff = finalScore - state.TransientScore;
                state.TransientScore = finalScore;

                view.GetBooster(booster.Id).GetScoringVisualizer().MultiplierApplied(randomMult, diff);
            }
        }
    }
}