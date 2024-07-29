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
            var multiplier = action.Factor.Get(state, booster).First();

            if (multiplier <= 0) 
                return;
            
            var boosterView = view.GetBooster(booster.Id);
                
            if (action.Products is { } products)
            {
                state.TransientScore += products * multiplier;
                boosterView.GetScoringVisualizer().ProductApplied(products * multiplier);
            }

            if (action.Multiplier is { } xMult)
            {
                var multiplied = 1 + xMult * multiplier;
                var finalProducts = state.TransientScore * multiplied;
                var diff = finalProducts - state.TransientScore;
                state.TransientScore = finalProducts;
                boosterView.GetScoringVisualizer().MultiplierApplied(multiplied, diff);
            }
        }
    }
}