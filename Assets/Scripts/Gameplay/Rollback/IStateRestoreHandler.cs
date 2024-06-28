namespace RogueIslands.Gameplay.Rollback
{
    public interface IStateRestoreHandler
    {
        void Restore(GameState backup, GameState current);
    }
}