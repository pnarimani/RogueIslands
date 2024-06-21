using System.Collections.Generic;
using Random = Unity.Mathematics.Random;

public static class Shuffler
{
    public static void Shuffle<T>(this IList<T> list, Random random)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = random.NextInt(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}