using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class DestroyBoosterExecutor : GameActionExecutor<DestroyBoosterAction>
    {
        private BoosterManagement _boosterManagement;

        public DestroyBoosterExecutor(BoosterManagement boosterManagement)
        {
            _boosterManagement = boosterManagement;
        }
        
        protected override void Execute(GameState state, IGameView view, IBooster booster, DestroyBoosterAction action)
        {
            if (action.Self)
            {
                _boosterManagement.DestroyBooster(booster.Id);
            }
            else
            {
                var index = state.Boosters.IndexOf((BoosterCard)booster);
                if (index < state.Boosters.Count - 1)
                {
                    var nextBooster = state.Boosters[index + 1];
                    _boosterManagement.DestroyBooster(nextBooster.Id);
                }
            }
        }
    }
}