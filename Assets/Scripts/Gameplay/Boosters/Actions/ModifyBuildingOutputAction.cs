using RogueIslands.Autofac;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class ModifyBuildingOutputAction : GameAction
    {
        public double? ProductMultiplier { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            if (state.CurrentEvent is not BuildingPlacedEvent buildingPlaced)
                return false;

            if (ProductMultiplier is { } mult)
            {
                buildingPlaced.Building.Output *= mult;
                buildingPlaced.Building.OutputUpgrade *= mult;
                return true;
            }

            return false;
        }
    }
}