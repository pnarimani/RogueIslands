using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters;

namespace RogueIslands.Gameplay.DryRun
{
    public class DryRunBoosterView : IBoosterView, IBoosterScoreVisualizer, IBoosterMoneyVisualizer,
        IBoosterScalingVisualizer, IBoosterRetriggerVisualizer, IBoosterResetVisualizer
    {
        private readonly List<int> _moneyChanges = new();
        private readonly List<double> _multipliers = new();
        private readonly List<double> _products = new();
        private readonly IBoosterView _realBoosterView;

        public DryRunBoosterView(IBoosterView realBoosterView)
        {
            _realBoosterView = realBoosterView;
        }

        private int ScaleUpTriggers { get; set; }
        private int ScaleDownTriggers { get; set; }
        private int Retriggers { get; set; }
        private int Resets { get; set; }

        public void Play(int moneyChange)
            => _moneyChanges.Add(moneyChange);

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

        public void MultiplierApplied(double multiplier, double products) => _multipliers.Add(multiplier);

        public void ProductApplied(double products) => _products.Add(products);

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

        public void ApplyChanges(DryRunBoosterView lastFrameView, DryRunBoosterView noProbabilitiesView)
        {
            if (!_moneyChanges.SequenceEqual(lastFrameView._moneyChanges))
            {
                var moneyAndCount = _moneyChanges
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                var vis = _realBoosterView.GetMoneyVisualizer();
                if (!_moneyChanges.SequenceEqual(noProbabilitiesView._moneyChanges))
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunMoney(moneyAndCount);
            }

            if (!_multipliers.SequenceEqual(lastFrameView._multipliers))
            {
                var multipliersAndCount = _multipliers
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                var vis = _realBoosterView.GetScoringVisualizer();
                vis.HideDryRun();
                if (!_multipliers.SequenceEqual(noProbabilitiesView._multipliers))
                    vis.ShowDryRunMultiplyProbability();
                else
                    vis.ShowDryRunMultiplier(multipliersAndCount);
            }

            if (!_products.SequenceEqual(lastFrameView._products))
            {
                var productsAndCount = _products
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                var vis = _realBoosterView.GetScoringVisualizer();
                vis.HideDryRun();
                if (!_products.SequenceEqual(noProbabilitiesView._products))
                    vis.ShowDryRunAddProbability();
                else
                    vis.ShowDryRunProducts(productsAndCount);
            }

            if (ScaleUpTriggers != lastFrameView.ScaleUpTriggers)
            {
                var vis = _realBoosterView.GetScalingVisualizer();
                vis.HideDryRun();
                if (!_moneyChanges.SequenceEqual(noProbabilitiesView._moneyChanges))
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunScaleUp(ScaleUpTriggers);
            }

            if (ScaleDownTriggers != lastFrameView.ScaleDownTriggers)
            {
                var vis = _realBoosterView.GetScalingVisualizer();
                vis.HideDryRun();
                if (!_moneyChanges.SequenceEqual(noProbabilitiesView._moneyChanges))
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunScaleDown(ScaleDownTriggers);
            }

            if (Retriggers != lastFrameView.Retriggers)
            {
                var vis = _realBoosterView.GetRetriggerVisualizer();
                vis.HideDryRun();
                if (Retriggers != noProbabilitiesView.Retriggers)
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunRetriggers(Retriggers);
            }

            if (Resets != lastFrameView.Resets)
            {
                var vis = _realBoosterView.GetResetVisualizer();
                vis.HideDryRun();
                if (Resets != noProbabilitiesView.Resets)
                    vis.ShowDryRunProbability();
                else
                    vis.ShowDryRunReset();
            }
        }

        public void HideAll()
        {
            _realBoosterView.GetMoneyVisualizer().HideDryRun();
            _realBoosterView.GetScoringVisualizer().HideDryRun();
            _realBoosterView.GetRetriggerVisualizer().HideDryRun();
            _realBoosterView.GetResetVisualizer().HideDryRun();
        }
    }
}