using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters.Conditions;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters.Evaluators
{
    public class ColorCheckEvaluator : GameConditionEvaluator<ColorCheckCondition>
    {
        private readonly HashSet<ColorTag> _existingColors = new();

        protected override bool Evaluate(GameState state, IBooster booster, ColorCheckCondition condition)
        {
            _existingColors.Clear();
            foreach (var tag in state.Clusters.SelectMany(x => x).Select(b => b.Color))
                _existingColors.Add(tag);

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