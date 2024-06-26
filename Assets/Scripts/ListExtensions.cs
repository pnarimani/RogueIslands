using System.Collections.Generic;
using Random = Unity.Mathematics.Random;

namespace RogueIslands
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list, ref Random random)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.NextInt(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static T SelectRandom<T>(this IReadOnlyList<T> list, ref Random rand)
        {
            return list[rand.NextInt(list.Count)];
        }
    }
}