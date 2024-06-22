using RogueIslands.Boosters;

namespace RogueIslands.Rollback
{
    public class BoosterRestoreHandler : IStateRestoreHandler
    {
        public void Restore(GameState backup, GameState state)
        {
            foreach (var booster in state.Boosters)
            {
                var backupData = backup.Boosters.Find(b => AreEqual(b, booster));
                if (backupData != null)
                    booster.EventAction = backupData.EventAction;
            }

            foreach (var booster in state.WorldBoosters)
            {
                var backupData = backup.WorldBoosters.Find(b => AreEqual(b, booster));
                if (backupData != null)
                    booster.EventAction = backupData.EventAction;
            }
        }
        
        private static bool AreEqual(BoosterCard b1, BoosterCard b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;
        
        private static bool AreEqual(WorldBooster b1, WorldBooster b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;
    }
}