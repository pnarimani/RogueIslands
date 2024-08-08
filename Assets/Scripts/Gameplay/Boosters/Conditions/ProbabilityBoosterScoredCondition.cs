using System.Linq;
using RogueIslands.Autofac;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class ProbabilityBoosterScoredCondition : IGameCondition
    {
        public bool Evaluate(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            if (state.CurrentEvent is not BoosterScoredEvent boosterScored)
                return false;

            if (boosterScored.Booster.EventAction is null)
                return false;

            using var gameConditions = boosterScored.Booster.EventAction.GetAllConditions();
            
            return gameConditions.Any(c => c is ProbabilityCondition);
        }
    }
}