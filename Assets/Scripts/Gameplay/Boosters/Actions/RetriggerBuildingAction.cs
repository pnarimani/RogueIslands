using RogueIslands.Autofac;
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
        
        protected override bool ExecuteAction(IBooster booster)
        {
            if (RemainingCharges is <= 0)
                return false;

            if (RemainingTriggers <= 0)
            {
                if (RemainingCharges != null)
                    RemainingCharges--;
                return false;
            }

            RemainingTriggers--;

            Buildings ??= new BuildingFromCurrentEvent();

            var view = StaticResolver.Resolve<IGameView>();
            var scoringController = StaticResolver.Resolve<ScoringController>();
            view.GetBooster(booster.Id).GetRetriggerVisualizer().PlayRetrigger();

            foreach (var building in Buildings.Get(booster))
            {
                scoringController.TriggerBuilding(building);
            }

            return true;
        }
    }
}