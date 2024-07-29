using RogueIslands.Autofac;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters.Executors
{
    public class DestroyBoosterExecutor : GameActionExecutor<DestroyBoosterAction>
    {
        protected override void Execute(GameState state, IGameView view, IBooster booster, DestroyBoosterAction action)
        {
            var boosterManagement = StaticResolver.Resolve<BoosterManagement>();
            if (action.Self)
            {
                boosterManagement.DestroyBooster(booster.Id);
            }
            else
            {
                var index = state.Boosters.IndexOf((BoosterCard)booster);
                if (index < state.Boosters.Count - 1)
                {
                    var nextBooster = state.Boosters[index + 1];
                    boosterManagement.DestroyBooster(nextBooster.Id);
                }
            }
        }
    }
}