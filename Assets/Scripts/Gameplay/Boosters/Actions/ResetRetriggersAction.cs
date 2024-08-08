using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ResetRetriggersAction : GameAction
    {
        public ResetRetriggersAction() =>
            Condition = new GameEventCondition<ResetRetriggersEvent>();

        protected override bool ExecuteAction(IBooster booster)
        {
            var retrigger = booster.GetEventAction<RetriggerBuildingAction>();
            retrigger.RemainingTriggers = retrigger.RetriggerTimes;
            return true;
        }
    }
}