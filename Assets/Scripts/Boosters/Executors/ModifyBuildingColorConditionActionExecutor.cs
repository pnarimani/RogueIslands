using System.Collections.Generic;
using System.Linq;
using RogueIslands.Boosters.Actions;

namespace RogueIslands.Boosters.Executors
{
    public class ModifyBuildingColorConditionActionExecutor
        : ModifyConditionActionExecutor<BuildingColorCondition, ModifyBuildingColorConditionAction>
    {
        protected override void ModifyCondition(BuildingColorCondition condition)
        {
            var colors = condition.Colors as List<ColorTag> ?? condition.Colors.ToList();
            Mangle(colors, ColorTag.Blue, ColorTag.Red);
            Mangle(colors, ColorTag.Red, ColorTag.Blue);
            Mangle(colors, ColorTag.Black, ColorTag.White);
            Mangle(colors, ColorTag.White, ColorTag.Black);
            condition.Colors = colors;
        }

        private static void Mangle(List<ColorTag> colors, ColorTag toExist, ColorTag toAdd)
        {
            if (colors.Contains(toExist) && !colors.Contains(toAdd))
                colors.Add(toAdd);
        }
    }
}