using System.Collections.Generic;
using System.Linq;
using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class BuildingSizeCondition : IGameConditionWithSource<Building>
    {
        public ISource<Building> Source { get; set; }
        public IReadOnlyList<BuildingSize> Allowed { get; set; }
        public IReadOnlyList<BuildingSize> Banned { get; set; }

        public BuildingSizeCondition()
        {
        }
        
        public BuildingSizeCondition(BuildingSize size)
        {
            Allowed = new List<BuildingSize> { size };
            Source = new BuildingFromCurrentEvent();
        }

        public bool Evaluate(IBooster booster)
        {
            Source ??= new BuildingFromCurrentEvent();
            
            foreach (var building in Source.Get(booster))
            {
                var inAllowed = Allowed == null || Allowed.Contains(building.Size);
                var notInBanned = Banned == null || !Banned.Contains(building.Size);

                if (!(inAllowed && notInBanned))
                    return false;
            }

            return true;
        }
    }
}