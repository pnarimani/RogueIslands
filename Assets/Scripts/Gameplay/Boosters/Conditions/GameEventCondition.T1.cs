using RogueIslands.Autofac;
using RogueIslands.Gameplay.GameEvents;

namespace RogueIslands.Gameplay.Boosters.Conditions
{
    public class GameEventCondition<T> : IGameCondition
    {
        public GameEventCondition Or<TOther>() where TOther : IGameEvent
        {
            return new GameEventCondition(typeof(T), typeof(TOther));
        }

        public bool Evaluate(IBooster booster)
        {
            var state = StaticResolver.Resolve<GameState>();
            return state.CurrentEvent is T;
        }
    }
}