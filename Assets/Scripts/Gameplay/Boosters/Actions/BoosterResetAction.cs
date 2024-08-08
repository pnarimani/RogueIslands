using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class BoosterResetAction : GameAction
    {
        public double? Product { get; set; }
        public double? Multiplier { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var scoringAction = booster.GetEventAction<ScoringAction>();
            scoringAction.Addition = Product;
            scoringAction.Multiplier = Multiplier;
            StaticResolver.Resolve<IGameView>().GetBooster(booster.Id).GetResetVisualizer().PlayReset();
            return true;
        }
    }
}