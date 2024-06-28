using System.Collections.Generic;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class ColorCheckCondition : IGameCondition
    {
        public IReadOnlyList<ColorTag> ForcedColors { get; set; }
        public IReadOnlyList<ColorTag> BannedColors { get; set; }
    }
}