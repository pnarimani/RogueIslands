using System.Collections.Generic;
using System.Linq;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters
{
    public class ColorCheckEvaluator : GameConditionEvaluator<ColorCheckCondition>
    {
        private readonly HashSet<ColorTag> _existingColors = new();
        protected override bool Evaluate(GameState state, IBooster booster, ColorCheckCondition condition)
        {
            _existingColors.Clear();
            foreach (var tag in state.Clusters.SelectMany(x => x).Select(b => b.Color))
                _existingColors.Add(tag);

            var exist = condition.ColorsToExist == null || _existingColors.All(color => condition.ColorsToExist.Contains(color));
            var nonExist = condition.ColorsToNotExist == null || _existingColors.All(color => !condition.ColorsToNotExist.Contains(color));
            return exist && nonExist;
        }
    }
}