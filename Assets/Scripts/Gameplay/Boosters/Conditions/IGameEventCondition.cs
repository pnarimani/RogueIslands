using System;
using System.Collections.Generic;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public interface IGameEventCondition : IGameCondition
    {
        IReadOnlyList<Type> TriggeringEvents { get; set; }
    }
}