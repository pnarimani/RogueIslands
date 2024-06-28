using RogueIslands.Gameplay.Boosters;

namespace RogueIslands.Gameplay.Rollback
{
    public class BoosterRestoreHandler : IStateRestoreHandler
    {
        public void Restore(GameState backup, GameState current)
        {
            foreach (var booster in current.Boosters)
            {
                var backupData = backup.Boosters.Find(b => AreEqual(b, booster));
                if (backupData != null)
                    booster.EventAction = backupData.EventAction;
            }
        }
        
        private static bool AreEqual(BoosterCard b1, BoosterCard b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;
    }
}