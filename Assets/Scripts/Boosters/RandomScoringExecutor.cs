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

        protected override void Execute(GameState state, IGameView view, IBooster booster, RandomScoringAction action)
            => state.ScoringState.Multiplier += _random.NextInt(0, (int)action.PlusMult);
    }
}