using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ScoringActionExecutor : GameActionExecutor<ScoringAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, ScoringAction action)
        {
            if (action.Products is { } products and > 0)
            {
                state.TransientScore += products;
                view.GetBooster(booster.Id).GetScoringVisualizer().ProductApplied(products);
            }

            if (action.Multiplier is { } xMult and > 1)
            {
                var final = state.TransientScore * xMult;
                var diff = final - state.TransientScore;
                state.TransientScore = final;
                view.GetBooster(booster.Id).GetScoringVisualizer().MultiplierApplied(xMult, diff);
            }
        }
    }
}