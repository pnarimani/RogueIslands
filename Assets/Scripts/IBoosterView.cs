using RogueIslands.Boosters;

namespace RogueIslands
{
    public interface IBoosterView
    {
        void OnBeforeActionExecuted(GameState state, GameAction action);
        void OnAfterActionExecuted(GameState state, GameAction action);

        void Remove();
        void UpdateDescription();
    }
}