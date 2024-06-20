namespace RogueIslands.Boosters
{
    public class DestroyBoosterExecutor : GameActionExecutor<DestroyBoosterAction>
    {
        protected override void Execute(GameState state, IGameView view, BoosterCard booster, DestroyBoosterAction action)
        {
            if (action.Self)
            {
                state.DestroyBooster(view, booster.Id);
            }
            else
            {
                var index = state.Boosters.IndexOf(booster);
                if (index < state.Boosters.Count - 1)
                {
                    var nextBooster = state.Boosters[index + 1];
                    state.DestroyBooster(view, nextBooster.Id);
                }
            }
        }
    }
}