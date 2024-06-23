using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters
{
    public class ColorCheckCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> ColorsToExist { get; set; }
        public IReadOnlyList<ColorTag> ColorsToNotExist { get; set; }
    }
}