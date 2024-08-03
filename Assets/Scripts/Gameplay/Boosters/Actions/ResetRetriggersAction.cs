﻿using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ResetRetriggersAction : GameAction
    {
        public ResetRetriggersAction() =>
            Conditions = new List<IGameCondition>
            {
                new GameEventCondition<ResetRetriggersEvent>(),
            };
    }
}