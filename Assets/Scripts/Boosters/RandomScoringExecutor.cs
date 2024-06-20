using Unity.Mathematics;

namespace RogueIslands.Boosters
{
    public class RandomScoringExecutor : GameActionExecutor<RandomScoringAction>
    {
        private Random _random;

        public RandomScoringExecutor(Random random)
        {
            _random = random;
        }

        protected override void Execute(GameState state, IGameView view, Booster booster, RandomScoringAction action) 
            => state.ScoringState.Multiplier += _random.NextDouble(0, action.PlusMult);
    }
}