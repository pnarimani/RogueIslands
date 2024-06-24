using System.Linq;
using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class WorldBoosterScalingExecutor : GameActionExecutor<WorldBoosterScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            WorldBoosterScalingAction action)
        {
            var worldBooster = (WorldBooster)booster;
            var scoring = booster.GetEventAction<ScoringAction>();

            // optimize tag
            var countInside = state.Clusters
                .SelectMany(x => x)
                .Count(b => (b.Position - worldBooster.Position).magnitude <= worldBooster.Range);
            var countOutside = state.Clusters
                .SelectMany(x => x)
                .Count(b => (b.Position - worldBooster.Position).magnitude <= worldBooster.Range);

            SetBoosterToStartingStats(action, scoring);
            ProcessInsideBuildings(action, scoring, countInside);
            ProcessOutsideBuildings(action, scoring, countOutside);
        }

        private static void SetBoosterToStartingStats(WorldBoosterScalingAction action, ScoringAction scoring)
        {
            scoring.Products = action.StartingProducts;
            scoring.PlusMult = action.StartingPlusMult;
            scoring.XMult = action.StartingXMult;
        }

        private static void ProcessOutsideBuildings(WorldBoosterScalingAction action, ScoringAction scoring,
            int countOutside)
        {
            if (action.ProductChangePerBuildingOutside is { } k)
                scoring.Products += k * countOutside;

            if (action.PlusMultChangePerBuildingOutside is { } l)
                scoring.PlusMult += l * countOutside;

            if (action.XMultChangePerBuildingOutside is { } m)
                scoring.XMult += m * countOutside;
        }

        private static void ProcessInsideBuildings(WorldBoosterScalingAction action, ScoringAction scoring,
            int countInside)
        {
            if (action.ProductChangePerBuildingInside is { } p)
                scoring.Products += p * countInside;

            if (action.PlusMultChangePerBuildingInside is { } q)
                scoring.PlusMult += q * countInside;

            if (action.XMultChangePerBuildingInside is { } r)
                scoring.XMult += r * countInside;
        }
    }
}