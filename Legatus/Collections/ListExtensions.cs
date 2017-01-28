using System;
using System.Collections.Generic;

namespace Legatus.Collections
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        /// <summary>
        /// Randomizes the order of items in the list using a Fisher-Yates shuffle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
