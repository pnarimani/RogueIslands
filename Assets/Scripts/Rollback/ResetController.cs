using System.Collections.Generic;
using RogueIslands.GameEvents;
using UnityEngine.Profiling;

namespace RogueIslands.Rollback
{
    public class ResetController
    {
        private readonly IReadOnlyList<IStateRestoreHandler> _restoreHandlers;
        private readonly EventController _eventController;
        private readonly GameState _state;
        
        private GameState _clone;

        public ResetController(GameState state, EventController eventController, IReadOnlyList<IStateRestoreHandler> restoreHandlers)
        {
            _state = state;
            _clone = _state.Clone();
            _eventController = eventController;
            _restoreHandlers = restoreHandlers;
        }

        public void RestoreProperties()
        {
            Profiler.BeginSample("RestoreProperties");
            Restore();
            Profiler.EndSample();

            Profiler.BeginSample("BackupProperties");
            _clone = _state.Clone();
            Profiler.EndSample();
            
            _eventController.Execute(new PropertiesRestored());
        }

        private void Restore()
        {
            if (_clone == null)
                return;

            foreach (var h in _restoreHandlers)
                h.Restore(_clone,_state);
            

            _clone = null;
        }
    }
}