using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Boosters.Conditions;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class ModifyBuildingColorConditionActionExecutor
        : ModifyConditionActionExecutor<BuildingColorCondition, ModifyBuildingColorConditionAction>
    {
        protected override void ModifyCondition(BuildingColorCondition condition)
        {
            var colors = condition.Colors as List<ColorTag> ?? condition.Colors.ToList();
            Mangle(colors, ColorTag.Blue, ColorTag.Red);
            Mangle(colors, ColorTag.Red, ColorTag.Blue);
            Mangle(colors, ColorTag.Purple, ColorTag.Green);
            Mangle(colors, ColorTag.Green, ColorTag.Purple);
            condition.Colors = colors;
        }

        private static void Mangle(List<ColorTag> colors, ColorTag toExist, ColorTag toAdd)
        {
            if (colors.Contains(toExist) && !colors.Contains(toAdd))
                colors.Add(toAdd);
        }
    }
}