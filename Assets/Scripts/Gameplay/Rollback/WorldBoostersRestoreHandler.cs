﻿using RogueIslands.Gameplay.Boosters;

namespace RogueIslands.Gameplay.Rollback
{
    public class WorldBoostersRestoreHandler : IStateRestoreHandler
    {
        public void Restore(GameState backup, GameState current)
        {
            current.WorldBoosters.SpawnDistribution = backup.WorldBoosters.SpawnDistribution;
            current.WorldBoosters.SpawnCount = backup.WorldBoosters.SpawnCount;

            foreach (var booster in current.WorldBoosters.SpawnedBoosters)
            {
                var backupData = backup.WorldBoosters.SpawnedBoosters.Find(b => AreEqual(b, booster));
                if (backupData != null)
                    booster.EventAction = backupData.EventAction;
            }
        }

        private static bool AreEqual(WorldBooster b1, WorldBooster b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;
    }
}