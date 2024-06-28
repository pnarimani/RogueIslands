namespace RogueIslands.Gameplay.Rollback
{
    public class GameplayRestoreHandler : IStateRestoreHandler
    {
        public void Restore(GameState backup, GameState current)
        {
            current.HandSize = backup.HandSize;
            current.TotalDays = backup.TotalDays;
            current.MoneyPayoutPerRound = backup.MoneyPayoutPerRound;
        }
    }
}