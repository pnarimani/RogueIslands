using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public static class GameRandomExtensions
    {
        public static RogueRandom GetRandomForType<T>(this GameState state)
        {
            if (!state.Randoms.TryGetValue(typeof(T).Name, out var random))
            {
                random = state.SeedRandom.NextRandom();
                state.Randoms[typeof(T).Name] = random;
            }
            
            return random;
        }
    }
}