﻿namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class RetriggerBuildingAction : GameAction
    {
        public int RetriggerTimes { get; set; } = 1;
        
        public int RemainingTriggers { get; set; }
    }
}