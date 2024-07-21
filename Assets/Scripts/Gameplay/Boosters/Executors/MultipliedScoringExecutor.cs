using System;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Buildings;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class MultipliedScoringExecutor : GameActionExecutor<MultipliedScoringAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster,
            MultipliedScoringAction action)
        {
            int multiplier;
            if (action.MultiplyByDay)
                multiplier = state.TotalDays - state.Day;
            else if (action.MultiplyByUniqueBuildings)
                multiplier = GetUniqueBuildingCount(state);
            else if (action.PerMoney is { } perMoney)
                multiplier = state.Money / perMoney;
            else if (action.MultiplyBySellValueOfBoosters)
                multiplier = state.Boosters.Sum(b => b.SellPrice);
            else if (action.MultiplyByRemainingCards)
                multiplier = state.Buildings.Deck.Count - state.Buildings.HandPointer;
            else if (action.MultiplyByRedBuildingsInRange)
            {
                multiplier = 0;
                var hasBadEyesight = state.HasBadEyesight();
                foreach (var b in state.GetInRangeBuildings(((BuildingEvent)state.CurrentEvent).Building))
                {
                    if (b.Color == ColorTag.Red)
                        multiplier++;

                    if (hasBadEyesight && b.Color == ColorTag.Blue)
                        multiplier++;
                }
            }
            else if (action.MultiplyByLargeBuildingsInRange)
            {
                multiplier = 0;
                foreach (var b in state.GetInRangeBuildings(((BuildingEvent)state.CurrentEvent).Building))
                {
                    if (b.Size == BuildingSize.Large)
                        multiplier++;
                }
            }
            else
            {
                multiplier = 0;
            }


            if (multiplier > 0)
            {
                var boosterView = view.GetBooster(booster);
                
                if (action.Products is { } products)
                {
                    state.TransientScore += products * multiplier;
                    boosterView.GetScoringVisualizer().ProductApplied(products * multiplier);
                }

                if (action.Multiplier is { } xMult)
                {
                    var multiplied = Math.Pow(xMult, multiplier);
                    var finalProducts = state.TransientScore * multiplied;
                    var diff = finalProducts - state.TransientScore;
                    state.TransientScore = finalProducts;
                    boosterView.GetScoringVisualizer().MultiplierApplied(multiplied, diff);
                }
            }
        }

        private static int GetUniqueBuildingCount(GameState state)
        {
            return state.PlacedDownBuildings
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