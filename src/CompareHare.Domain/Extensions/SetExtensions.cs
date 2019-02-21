#region usings

using System.Collections.Generic;
using System.Linq;

#endregion

namespace CompareHare.Domain.Extensions
{
    public static class SetExtensions
    {
        public static void UpdateTo<T>(this ISet<T> set, IReadOnlyCollection<T> items)
        {
            set.IntersectWith(items);
            set.AddAll(items.Except(set));
        }

        public static void RemoveRange<T>(this ISet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items) set.Remove(item);
        }

        public static void AddAll<T>(this ISet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items) set.Add(item);
        }

        public static HashSet<T> ToHashSet<T>(
            this IEnumerable<T> source,
            IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }
    }
}
