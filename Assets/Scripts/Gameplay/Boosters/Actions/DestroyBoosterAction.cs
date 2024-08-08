using RogueIslands.Autofac;

namespace RogueIslands.Gameplay.Boosters.Actions
{
    public class DestroyBoosterAction : GameAction
    {
        public bool Self { get; set; }

        protected override bool ExecuteAction(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            var boosterManagement = StaticResolver.Resolve<BoosterManagement>();

            if (Self)
            {
                boosterManagement.DestroyBooster(booster.Id);
                return true;
            }

            var index = state.Boosters.IndexOf((BoosterCard)booster);
            if (index < state.Boosters.Count - 1)
            {
                var nextBooster = state.Boosters[index + 1];
                boosterManagement.DestroyBooster(nextBooster.Id);
                return true;
            }

            return false;
        }
    }
}