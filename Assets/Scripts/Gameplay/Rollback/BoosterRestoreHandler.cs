using RogueIslands.Gameplay.Boosters;
using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Rollback
{
    public class BoosterRestoreHandler : IStateRestoreHandler
    {
        public void Restore(GameState backup, GameState current)
        {
            foreach (var booster in current.Boosters)
            {
                var backupData = backup.Boosters.Find(b => AreEqual(b, booster));
                if (backupData is not { EventAction: not null })
                    continue;

                RevertEventConditions(booster.EventAction, backupData.EventAction);
            }
        }

        private static void RevertEventConditions(GameAction currentEvent, GameAction backupEvent)
        {
            currentEvent.Conditions = backupEvent.Conditions;

            if (currentEvent is CompositeAction composite)
            {
                for (var i = 0; i < composite.Actions.Count; i++)
                    RevertEventConditions(composite.Actions[i], ((CompositeAction)backupEvent).Actions[i]);
            }
        }

        private static bool AreEqual(BoosterCard b1, BoosterCard b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;
    }
}