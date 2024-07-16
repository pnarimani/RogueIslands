using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class RatAttackExecutor : GameActionExecutor<RatAttack>
    {
        private BoosterManagement _boosterManagement;

        public RatAttackExecutor(BoosterManagement boosterManagement)
        {
            _boosterManagement = boosterManagement;
        }

        protected override void Execute(GameState state, IGameView view, IBooster booster, RatAttack action)
        {
            var index = state.Boosters.IndexOf((BoosterCard)booster);
            if (index >= state.Boosters.Count - 1)
                return;
            
            var nextBooster = state.Boosters[index + 1];
            var mult = nextBooster.SellPrice * 2;
            _boosterManagement.DestroyBooster(nextBooster.Id);

            booster.GetEventAction<ScoringAction>().PlusMult += mult;

            view.GetBooster(booster).OnAfterActionExecuted(state, new BoosterScalingAction());
        }
    }
}