using System.Collections.Generic;
using RogueIslands.Gameplay.Rand;

namespace RogueIslands.Gameplay
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list, RandomForAct random)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.NextInt(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static void Shuffle<T>(this IList<T> list, SeedRandom random)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.NextInt(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static T SelectRandom<T>(this IReadOnlyList<T> list, RandomForAct rand)
        {
            return list[rand.NextInt(list.Count)];
        }
    }
}