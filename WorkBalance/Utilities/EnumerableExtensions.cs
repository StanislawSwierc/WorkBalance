using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Utilities
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable)
        {
            return enumerable == null || enumerable.Count() == 0;
        }

        public static bool IsNullOrEmpty<T>(ICollection<T> enumerable)
        {
            return enumerable == null || enumerable.Count == 0;
        }
    }
}
