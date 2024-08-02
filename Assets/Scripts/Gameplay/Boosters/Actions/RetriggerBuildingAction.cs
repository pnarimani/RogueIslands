using RogueIslands.Gameplay.Boosters.Sources;
using RogueIslands.Gameplay.Buildings;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class RetriggerBuildingAction : GameAction
    {
        public ISource<Building> Buildings { get; set; }
        public int RetriggerTimes { get; set; } = 1;
        public int? RemainingCharges { get; set; }
        public int RemainingTriggers { get; set; }
    }
}