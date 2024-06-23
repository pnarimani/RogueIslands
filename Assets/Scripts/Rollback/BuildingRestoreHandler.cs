using RogueIslands.Buildings;

namespace RogueIslands.Rollback
{
    public class BuildingRestoreHandler : IStateRestoreHandler
    {
        public void Restore(GameState backup, GameState current)
        {
            foreach (var building in current.BuildingDeck.Deck)
            {
                var backupData = backup.BuildingDeck.Deck.Find(b => AreEqual(b, building));
                if (backupData != null)
                {
                    building.Range = backupData.Range;
                }
            }
        }

        private static bool AreEqual(Building b1, Building b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;
    }
}