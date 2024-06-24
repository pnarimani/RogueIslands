﻿using RogueIslands.Boosters.Actions;
using RogueIslands.GameEvents;

namespace RogueIslands.Boosters.Executors
{
    public class ModifyBuildingOutputExecutor : GameActionExecutor<ModifyBuildingOutputAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            ModifyBuildingOutputAction action)
        {
            if (state.CurrentEvent is not BuildingPlaced buildingPlaced) 
                return;

            if (action.ProductMultiplier is { } mult)
            {
                buildingPlaced.Building.Output *= mult;
                buildingPlaced.Building.OutputUpgrade *= mult;
            }
        }
    }
}