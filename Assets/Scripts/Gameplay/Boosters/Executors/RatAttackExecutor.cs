using RogueIslands.Gameplay.Boosters.Actions;
using RogueIslands.Gameplay.Rand;

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
            if (state.Boosters.Count > 1)
            {
                var randomForAct = state.GetRandomForType<RatAttack>().ForAct(state.Act);
                BoosterCard toDestroy;
                do
                {
                    toDestroy = state.Boosters.SelectRandom(randomForAct);
                } while (Equals(toDestroy, booster));

                _boosterManagement.DestroyBooster(toDestroy.Id);
            }

            booster.GetEventAction<ScoringAction>().Multiplier += 4;
            view.GetBooster(booster.Id).GetScalingVisualizer().PlayScaleUp();
        }
    }
}