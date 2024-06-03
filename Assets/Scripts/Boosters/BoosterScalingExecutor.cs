using System;

namespace RogueIslands.Boosters
{
    public class BoosterScalingExecutor : GameActionExecutor<BoosterScalingAction>
    {
        protected override void Execute(GameState state, IGameView view, Booster booster, BoosterScalingAction action)
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

        private static void ScaleScoringAction(BoosterScalingAction action, ScoringAction scoringAction)
        {
            scoringAction.Products += action.ProductChange;
            scoringAction.PlusMult += action.PlusMultChange;
            scoringAction.XMult += action.XMultChange;
        }
    }
}