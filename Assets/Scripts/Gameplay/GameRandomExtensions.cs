using System;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public static class GameRandomExtensions
    {
        public static RogueRandom GetRandomForType<T>(this GameState state)
        {
            if (!state.Randoms.TryGetValue(typeof(T).Name, out var random))
            {
                HashCode hashCode = new();
                hashCode.Add(state.Seed);
                hashCode.Add(typeof(T).Name);
                random = new RogueRandom((uint)hashCode.ToHashCode());
                state.Randoms[typeof(T).Name] = random;
            }
            
            return random;
        }
    }
}