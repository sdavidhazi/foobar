using System;
using System.Collections.Generic;
using Impl.Test.Random;

namespace Impl.Test.Extensions
{
    static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list, IRandomProvider random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
