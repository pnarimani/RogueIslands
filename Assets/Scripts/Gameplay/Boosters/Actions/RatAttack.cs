using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class RatAttack : GameAction
    {
        public double Change { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var view = StaticResolver.Resolve<IGameView>();
            var boosterManagement = StaticResolver.Resolve<BoosterManagement>();

            if (state.Boosters.Count > 1)
            {
                var randomForAct = state.GetRandomForType<RatAttack>().ForAct(state.Act);
                BoosterCard toDestroy;
                do
                {
                    toDestroy = state.Boosters.SelectRandom(randomForAct);
                } while (Equals(toDestroy, booster));

                boosterManagement.DestroyBooster(toDestroy.Id);
            }

            booster.GetEventAction<ScoringAction>().Multiplier += Change;
            view.GetBooster(booster.Id).GetScalingVisualizer().PlayScaleUp();
            return true;
        }
    }
}