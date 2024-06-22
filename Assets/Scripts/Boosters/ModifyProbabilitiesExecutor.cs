using System.Collections.Generic;
using System.Linq;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters
{
    public class ModifyProbabilitiesExecutor : GameActionExecutor<ModifyProbabilitiesAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            ModifyProbabilitiesAction action)
        {
            if (state.CurrentEvent is PropertiesRestored)
            {
                foreach (var b in state.Boosters)
                {
                    foreach (var probability in b.EventAction.GetAllConditions().OfType<ProbabilityCondition>())
                    {
                        probability.FavorableOutcome *= 2;
                    }
                }

                foreach (var b in state.WorldBoosters)
                {
                    foreach (var probability in b.EventAction.GetAllConditions().OfType<ProbabilityCondition>())
                    {
                        probability.FavorableOutcome *= 2;
                    }
                }
            }
            else if(state.CurrentEvent is BoosterAdded boosterAdded)
            {
                if (Equals(boosterAdded.Booster, booster))
                    return;
                
                foreach (var probability in boosterAdded.Booster.EventAction.GetAllConditions()
                             .OfType<ProbabilityCondition>())
                {
                    probability.FavorableOutcome *= 2;
                }
            }
        }
    }
}