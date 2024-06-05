using System;
using System.Collections.Generic;

public static class Shuffler
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list, Random random = null)
    {
        random ??= rng;
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}