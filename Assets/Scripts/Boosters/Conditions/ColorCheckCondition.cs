using System.Collections.Generic;
using RogueIslands.Buildings;

namespace RogueIslands.Boosters
{
    public class ColorCheckCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> ForcedColors { get; set; }
        public IReadOnlyList<ColorTag> BannedColors { get; set; }
    }
}