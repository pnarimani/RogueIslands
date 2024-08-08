using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Sources
{
    public class GetBuildingSizes : ISource<BuildingSize>
    {
        public ISource<Building> Source { get; set; }

        public IEnumerable<BuildingSize> Get(IBooster booster) =>
            Source.Get(booster).Select(b => b.Size);
    }
}