using System;
using System.Linq;
using RogueIslands.Boosters.Actions;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters.Executors
{
    public class MultipliedScoringExecutor : GameActionExecutor<MultipliedScoringAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            MultipliedScoringAction action)
        {
            int multiplier;
            if (action.MultiplyByDay)
                multiplier = state.TotalDays - state.Day;
            else if (action.MultiplyByIslandCount)
                multiplier = state.Clusters.Count;
            else if (action.MultiplyByUniqueBuildings)
                multiplier = GetUniqueBuildingCount(state);
            else
                multiplier = 1;
            
            if (action.Products is { } products)
                state.ScoringState.Products += products * multiplier;
            if (action.PlusMult is { } plusMult)
                state.ScoringState.Multiplier += plusMult * multiplier;
            if (action.XMult is { } xMult)
                state.ScoringState.Multiplier *= xMult * multiplier;
        }

        private static int GetUniqueBuildingCount(GameState state)
        {
            return state.Clusters.SelectMany(x => x)
                .GroupBy(GetHash)
                .Count();
        }

        private static int GetHash(Building building)
        {
            var code = new HashCode();
            code.Add(building.Category);
            code.Add(building.Color);
            code.Add(building.Size);
            return code.ToHashCode();
        }
    }
}