namespace RogueIslands.Gameplay.Buildings
{
    public class DiscardController
    {
        private readonly GameState _state;
        private readonly IGameView _view;

        public DiscardController(GameState state, IGameView view)
        {
            _view = view;
            _state = state;
        }
        
        public void DiscardAll()
        {
            _view.DestroyBuildingsInHand();
            _state.Buildings.HandPointer += _state.HandSize;
            _view.ShowBuildingsInHand();
        }
    }
}