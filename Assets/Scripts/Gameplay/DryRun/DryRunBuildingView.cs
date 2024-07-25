using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.DryRun
{
    public class DryRunBuildingView : IBuildingView
    {
        private readonly List<int> _bonusScores = new();
        private readonly IBuildingView _realView;

        private readonly List<int> _triggerScores = new();

        public DryRunBuildingView(IBuildingView realView) => _realView = realView;

        public void BuildingTriggered(int score)
        {
            _triggerScores.Add(score);
        }

        public void BonusTriggered(int score)
        {
            _bonusScores.Add(score);
        }

        public void ShowDryRunTrigger(Dictionary<int, int> triggerAndCount)
        {
            throw new InvalidOperationException();
        }

        public void ShowDryRunBonus(Dictionary<int, int> bonusAndCount)
        {
            throw new InvalidOperationException();
        }

        public void HideAllDryRunLabels()
        {
            throw new InvalidOperationException();
        }

        public void ApplyChanges(DryRunBuildingView lastFrame)
        {
            if (_realView == null)
                return;
            
            if (!_triggerScores.SequenceEqual(lastFrame._triggerScores))
            {
                var productsAndCount = _triggerScores
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                _realView.HideAllDryRunLabels();
                _realView.ShowDryRunTrigger(productsAndCount);
            }

            if (!_bonusScores.SequenceEqual(lastFrame._bonusScores))
            {
                var productsAndCount = _bonusScores
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());

                _realView.HideAllDryRunLabels();
                _realView.ShowDryRunBonus(productsAndCount);
            }
        }

        public void HideAll()
        {
            if (_realView == null)
                return;
            
            _realView.HideAllDryRunLabels();
        }
    }
}