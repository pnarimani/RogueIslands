using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class RatAttackExecutor : GameActionExecutor<RatAttack>
    {
        private BoosterManagement _boosterManagement;
        private RogueRandom _rogueRandom = new(3);

        public RatAttackExecutor(BoosterManagement boosterManagement)
        {
            _boosterManagement = boosterManagement;
        }

        protected override void Execute(GameState state, IGameView view, IBooster booster, RatAttack action)
        {
            if (state.Boosters.Count > 1)
            {
                var toDestroy = state.Boosters.SelectRandom(_rogueRandom.ForAct(state.Act));
                _boosterManagement.DestroyBooster(toDestroy.Id);
            }

            booster.GetEventAction<ScoringAction>().Multiplier += 4;
            view.GetBooster(booster).GetScalingVisualizer().PlayScaleUp();
        }
    }
}