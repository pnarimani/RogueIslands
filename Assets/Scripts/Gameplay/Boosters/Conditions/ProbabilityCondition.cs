using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class ProbabilityCondition : IGameCondition
    {
        public int FavorableOutcome { get; set; }
        public int TotalOutcomes { get; set; }

        public ProbabilityCondition()
        {
        }

        public ProbabilityCondition(int favorableOutcome, int totalOutcomes)
        {
            FavorableOutcome = favorableOutcome;
            TotalOutcomes = totalOutcomes;
        }

        public bool Evaluate(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var rand = state.GetRandomForType<ProbabilityCondition>().ForAct(state.Act);
            var roll = rand.NextInt(0, TotalOutcomes);
            return roll < FavorableOutcome * (state.GetRiggedCount() + 1);
        }
    }
}