using System.Collections.Generic;
using UnityEngine.Profiling;

namespace RogueIslands.Rollback
{
    public static class StateReset
    {
        private static GameState _clone;
        private static IReadOnlyList<IStateRestoreHandler> _restoreHandlers;

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

            _restoreHandlers ??= StaticResolver.Resolve<IReadOnlyList<IStateRestoreHandler>>();
            foreach (var h in _restoreHandlers)
                h.Restore(_clone,state);
            

            _clone = null;
        }
    }
}