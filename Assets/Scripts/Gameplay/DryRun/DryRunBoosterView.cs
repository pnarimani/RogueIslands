using System;
using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters;

namespace RogueIslands.Gameplay.DryRun
{
    public class DryRunBoosterView : IBoosterView, IBoosterScoreVisualizer, IBoosterMoneyVisualizer,
        IBoosterScalingVisualizer, IBoosterRetriggerVisualizer, IBoosterResetVisualizer
    {
        public readonly List<int> MoneyChanges = new();
        public readonly List<double> Multipliers = new();
        public readonly List<double> Products = new();

        public int ScaleUpTriggers { get; set; }
        public int ScaleDownTriggers { get; set; }
        public int Retriggers { get; set; }
        public int Resets { get; set; }

        public void Play(int moneyChange)
            => MoneyChanges.Add(moneyChange);

        public void ShowDryRunMoney(Dictionary<int, int> moneyAndCount)
            => throw new InvalidOperationException();

        public void ShowDryRunReset() => throw new InvalidOperationException();

        public void ShowDryRunProbability() => throw new InvalidOperationException();

        public void PlayReset() => Resets++;

        public void ShowDryRunRetriggers(int count) => throw new InvalidOperationException();

        public void PlayRetrigger() => Retriggers++;

        public void ShowDryRunScaleUp(int count) => throw new InvalidOperationException();

        public void ShowDryRunScaleDown(int count) => throw new InvalidOperationException();

        public void PlayScaleUp() => ScaleUpTriggers++;

        public void PlayScaleDown() => ScaleDownTriggers++;

        public void ShowDryRunMultiplyProbability() => throw new InvalidOperationException();

        public void HideDryRun() => throw new InvalidOperationException();

        public void ShowDryRunProducts(Dictionary<double, int> productsAndCount) =>
            throw new InvalidOperationException();

        public void ShowDryRunAddProbability() => throw new InvalidOperationException();

        public void MultiplierApplied(double multiplier, double products) => Multipliers.Add(multiplier);

        public void ProductApplied(double products) => Products.Add(products);

        public void ShowDryRunMultiplier(Dictionary<double, int> multipliersAndCount)
            => throw new InvalidOperationException();

        public void Remove()
        {
        }

        public IBoosterResetVisualizer GetResetVisualizer() => this;

        public IBoosterRetriggerVisualizer GetRetriggerVisualizer() => this;

        public IBoosterScoreVisualizer GetScoringVisualizer() => this;

        public IBoosterScalingVisualizer GetScalingVisualizer() => this;

        public IBoosterMoneyVisualizer GetMoneyVisualizer() => this;
    }
}