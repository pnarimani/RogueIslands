using System.Linq;
using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.DryRun
{
    public static class BoosterComparer
    {
        public static void Compare(
            IBooster booster,
            IBoosterView realView,
            DryRunBoosterView allProbabilities,
            DryRunBoosterView noProbabilities,
            DryRunBoosterView lastFrame)
        {
            if (!allProbabilities.MoneyChanges.SequenceEqual(lastFrame.MoneyChanges))
            {
                var moneyAndCount = allProbabilities.MoneyChanges
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                var vis = realView.GetMoneyVisualizer();
                vis.HideDryRun();
                if (!allProbabilities.MoneyChanges.SequenceEqual(noProbabilities.MoneyChanges))
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunMoney(moneyAndCount);
            }

            if (!allProbabilities.Multipliers.SequenceEqual(lastFrame.Multipliers))
            {
                var multipliersAndCount = allProbabilities.Multipliers
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                var vis = realView.GetScoringVisualizer();
                vis.HideDryRun();
                if (!allProbabilities.Multipliers.SequenceEqual(noProbabilities.Multipliers) || IsRandom(booster))
                    vis.ShowDryRunMultiplyProbability();
                else
                    vis.ShowDryRunMultiplier(multipliersAndCount);
            }

            if (!allProbabilities.Products.SequenceEqual(lastFrame.Products))
            {
                var productsAndCount = allProbabilities.Products
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                var vis = realView.GetScoringVisualizer();
                vis.HideDryRun();
                if (!allProbabilities.Products.SequenceEqual(noProbabilities.Products) || IsRandom(booster))
                    vis.ShowDryRunAddProbability();
                else
                    vis.ShowDryRunProducts(productsAndCount);
            }

            if (false && allProbabilities.ScaleUpTriggers != lastFrame.ScaleUpTriggers)
            {
                var vis = realView.GetScalingVisualizer();
                vis.HideDryRun();
                if (!allProbabilities.MoneyChanges.SequenceEqual(noProbabilities.MoneyChanges))
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunScaleUp(allProbabilities.ScaleUpTriggers);
            }

            if (false && allProbabilities.ScaleDownTriggers != lastFrame.ScaleDownTriggers)
            {
                var vis = realView.GetScalingVisualizer();
                vis.HideDryRun();
                if (!allProbabilities.MoneyChanges.SequenceEqual(noProbabilities.MoneyChanges))
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunScaleDown(allProbabilities.ScaleDownTriggers);
            }

            if (allProbabilities.Retriggers != lastFrame.Retriggers)
            {
                var vis = realView.GetRetriggerVisualizer();
                vis.HideDryRun();
                if (allProbabilities.Retriggers != noProbabilities.Retriggers)
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunRetriggers(allProbabilities.Retriggers);
            }

            if (allProbabilities.Resets != lastFrame.Resets)
            {
                var vis = realView.GetResetVisualizer();
                vis.HideDryRun();
                if (allProbabilities.Resets != noProbabilities.Resets)
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunReset();
            }
        }

        private static bool IsRandom(IBooster booster) => booster.GetEventAction<RandomScoringAction>() != null;
    }
}