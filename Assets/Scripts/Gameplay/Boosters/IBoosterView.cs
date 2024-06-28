using RogueIslands.Gameplay.Boosters.Actions;

namespace RogueIslands.Gameplay.Boosters
{
    public interface IBoosterView
    {
        void OnBeforeActionExecuted(GameState state, GameAction action);
        void OnAfterActionExecuted(GameState state, GameAction action);

        void Remove();
    }
}