using System.Collections.Generic;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class ColorCheckCondition : IGameConditionWithSource<Building>
    {
        public ISource<Building> Source { get; set; }
        public IReadOnlyList<ColorTag> ForcedColors { get; set; }
        public IReadOnlyList<ColorTag> BannedColors { get; set; }

        public ColorCheckCondition()
        {
        }

        public ColorCheckCondition(ColorTag forceColor)
        {
            ForcedColors = new List<ColorTag> { forceColor };
        }
        
        public ColorCheckCondition(ColorTag forceColor, ISource<Building> source)
        {
            ForcedColors = new List<ColorTag> { forceColor };
            Source = source;
        }
    }
}