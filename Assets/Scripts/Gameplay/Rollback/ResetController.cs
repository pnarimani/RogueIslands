using System.Collections.Generic;
using RogueIslands.Gameplay.GameEvents;
using RogueIslands.Serialization;

namespace RogueIslands.Gameplay.Rollback
{
    public class ResetController
    {
        private readonly IReadOnlyList<IStateRestoreHandler> _restoreHandlers;
        private readonly EventController _eventController;
        private readonly GameState _state;
        private readonly ICloner _cloner;
        
        private GameState _backup;

        public ResetController(GameState state, ICloner cloner, EventController eventController, IReadOnlyList<IStateRestoreHandler> restoreHandlers)
        {
            _cloner = cloner;
            _state = state;
            _backup = _cloner.Clone(_state);
            _eventController = eventController;
            _restoreHandlers = restoreHandlers;
        }

        public void RestoreProperties()
        {
            Restore();

            _backup = _cloner.Clone(_state);
            
            _eventController.Execute(new PropertiesRestored());
        }

        private void Restore()
        {
            if (_backup == null)
                return;

            foreach (var h in _restoreHandlers)
                h.Restore(_backup,_state);
            
            _backup = null;
        }
    }
}