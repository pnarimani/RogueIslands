using System;
using RogueIslands.Boosters;
using UnityEngine;
using UnityEngine.Profiling;

namespace RogueIslands
{
    public static class StateReset
    {
        private static GameState _clone;

        public static void RestoreProperties(this GameState state)
        {
            Profiler.BeginSample("RestoreProperties");
            state.Restore();
            Profiler.EndSample();

            Profiler.BeginSample("BackupProperties");
            _clone = state.Clone();
            Profiler.EndSample();
        }

        private static void Restore(this GameState state)
        {
            if (_clone == null)
                return;

            state.HandSize = _clone.HandSize;
            state.TotalDays = _clone.TotalDays;
            state.MoneyPayoutPerWeek = _clone.MoneyPayoutPerWeek;

            foreach (var building in state.BuildingDeck.Deck)
            {
                building.Range = _clone.BuildingDeck.Deck.Find(backup => AreEqual(backup, building)).Range;
            }

            foreach (var booster in state.Boosters)
            {
                var backup = _clone.Boosters.Find(backup => AreEqual(backup, booster));
                if (backup != null)
                    booster.EventAction = backup.EventAction;
            }

            foreach (var booster in state.WorldBoosters)
            {
                var backup = _clone.WorldBoosters.Find(backup => AreEqual(backup, booster));
                if (backup != null)
                    booster.EventAction = backup.EventAction;
            }

            _clone = null;
        }

        private static bool AreEqual(BoosterCard b1, BoosterCard b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;

        private static bool AreEqual(Building b1, Building b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;

        private static bool AreEqual(WorldBooster b1, WorldBooster b2)
            => b1.Id == b2.Id && b1.Name == b2.Name;
    }
}