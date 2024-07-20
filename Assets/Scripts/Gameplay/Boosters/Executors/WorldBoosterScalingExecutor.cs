using System.Linq;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class WorldBoosterScalingExecutor : GameActionExecutor<WorldBoosterScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            WorldBoosterScalingAction action)
        {
            var worldBooster = (WorldBooster)booster;
            var scoring = booster.GetEventAction<ScoringAction>();

            // optimize tag
            var countInside = state.PlacedDownBuildings
                .Count(b => (b.Position - worldBooster.Position).magnitude <= worldBooster.Range);
            var countOutside = state.PlacedDownBuildings
                .Count(b => (b.Position - worldBooster.Position).magnitude > worldBooster.Range);

            SetBoosterToStartingStats(action, scoring);
            ProcessInsideBuildings(action, scoring, countInside);
            ProcessOutsideBuildings(action, scoring, countOutside);
        }

        private static void SetBoosterToStartingStats(WorldBoosterScalingAction action, ScoringAction scoring)
        {
            scoring.Products = action.StartingProducts;
            scoring.Multiplier = action.StartingMultiplier;
        }

        private static void ProcessOutsideBuildings(WorldBoosterScalingAction action, ScoringAction scoring,
            int countOutside)
        {
            if (action.ProductChangePerBuildingOutside is { } k)
                scoring.Products += k * countOutside;
            
            if (action.XMultChangePerBuildingOutside is { } m)
                scoring.Multiplier += m * countOutside;
        }

        private static void ProcessInsideBuildings(WorldBoosterScalingAction action, ScoringAction scoring,
            int countInside)
        {
            if (action.ProductChangePerBuildingInside is { } p)
                scoring.Products += p * countInside;
            
            if (action.XMultChangePerBuildingInside is { } r)
                scoring.Multiplier += r * countInside;
        }
    }
}