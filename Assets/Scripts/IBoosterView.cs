using RogueIslands.Boosters;

namespace RogueIslands
{
    public interface IBoosterView
    {
        void OnActionExecuted(GameState state, GameAction action);
    }
}