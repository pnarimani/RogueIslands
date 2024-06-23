using RogueIslands.Boosters;
using RogueIslands.Boosters.Actions;

namespace RogueIslands
{
    public interface IBoosterView
    {
        void OnBeforeActionExecuted(GameState state, GameAction action);
        void OnAfterActionExecuted(GameState state, GameAction action);

        void Remove();
    }
}