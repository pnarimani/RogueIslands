using System;
using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Evaluators
{
    public class ColorCheckEvaluator : GameConditionEvaluator<ColorCheckCondition>
    {
        private readonly HashSet<ColorTag> _existingColors = new();

        protected override bool Evaluate(GameState state, IBooster booster, ColorCheckCondition condition)
        {
            condition.Source ??= new BuildingFromCurrentEvent();
            
            _existingColors.Clear();
            foreach (var tag in condition.Source.Get(state, booster).Select(b => b.Color))
                _existingColors.Add(tag);

            if (state.HasBadEyesight())
            {
                if (_existingColors.Contains(ColorTag.Red))
                    _existingColors.Add(ColorTag.Blue);
                if (_existingColors.Contains(ColorTag.Blue))
                    _existingColors.Add(ColorTag.Red);
                if (_existingColors.Contains(ColorTag.Green))
                    _existingColors.Add(ColorTag.Purple);
                if (_existingColors.Contains(ColorTag.Purple))
                    _existingColors.Add(ColorTag.Green);
            }

            if (condition.ForcedColors != null)
            {
                foreach (var force in condition.ForcedColors)
                {
                    if (!_existingColors.Contains(force))
                        return false;
                }
            }

            if (condition.BannedColors != null)
            {
                foreach (var color in _existingColors)
                {
                    if (condition.BannedColors.Contains(color))
                        return false;
                }
            }

            return true;
        }
    }
}