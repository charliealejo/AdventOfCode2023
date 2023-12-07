﻿namespace AdventOfCode2023
{
    internal static class Extensions
    {
        internal static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length) =>
            length == 1
                ? list.Select(t => new T[] { t })
                : GetPermutations(list, length - 1)
                    .SelectMany(t => list.Where(e => !t.Contains(e)),
                        (t1, t2) => t1.Concat(new T[] { t2 }));

        internal static long Product(this IEnumerable<long> list) =>
            list.Aggregate(1L, (current, item) => current * item);

        internal static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source) =>
            source.Select((item, index) => (item, index));
    }
}