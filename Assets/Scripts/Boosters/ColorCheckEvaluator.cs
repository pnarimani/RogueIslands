using System.Collections.Generic;
using System.Linq;

namespace RogueIslands.Boosters
{
    public class ColorCheckEvaluator : ConditionEvaluator<ColorCheckCondition>
    {
        private readonly HashSet<string> _existingColors = new();
        protected override bool Evaluate(GameState state, IBooster booster, ColorCheckCondition condition)
        {
            _existingColors.Clear();
            foreach (var (tag, _) in state.Islands.SelectMany(x => x).Select(b => b.Color))
                _existingColors.Add(tag);

            var exist = condition.ColorsToExist == null || condition.ColorsToExist.All(color => _existingColors.Contains(color.Tag));
            var nonExist = condition.ColorsToNotExist == null || condition.ColorsToNotExist.All(color => !_existingColors.Contains(color.Tag));
            return exist && nonExist;
        }
    }
}