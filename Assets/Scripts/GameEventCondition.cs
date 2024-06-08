﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RogueIslands
{
    public class GameEventCondition : IGameCondition
    {
        public IReadOnlyList<string> TriggeringEvents { get; }

        public GameEventCondition(string triggeringEvent) 
            => TriggeringEvents = new List<string> { triggeringEvent };
        
        [JsonConstructor]
        public GameEventCondition(IReadOnlyList<string> triggeringEvents) 
            => TriggeringEvents = triggeringEvents;
    }
}