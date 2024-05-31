using System;

namespace RogueIslands.Boosters
{
    public class BoosterResetExecutor : GameActionExecutor<BoosterResetAction>
    {
        protected override void Execute(GameState state, Booster booster, BoosterResetAction action)
        {
            var scoringAction = booster.GetEventAction<ScoringAction>();
            ScaleScoringAction(action, scoringAction);
            UpdateBoosterDescription(booster, scoringAction);
        }

        private void UpdateBoosterDescription(Booster booster, ScoringAction scoringAction)
        {
            var index = booster.Description.IndexOf("Current:", StringComparison.Ordinal);
            if (index != -1)
                booster.Description = booster.Description.Substring(0, index);

            if (scoringAction.Products > 0)
                booster.Description += $"Current: {scoringAction.Products} products.";
            if (scoringAction.PlusMult > 0)
                booster.Description += $"Current: +{scoringAction.PlusMult} mult.";
            if (scoringAction.XMult > 1)
                booster.Description += $"Current: x{scoringAction.XMult:F2} mult.";
        }

        private static void ScaleScoringAction(BoosterResetAction action, ScoringAction scoringAction)
        {
            scoringAction.Products = action.Product;
            scoringAction.PlusMult = action.PlusMult;
            scoringAction.XMult = action.XMult;
        }
    }
}