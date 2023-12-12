namespace AdventOfCode2023
{
    internal static class Extensions
    {
        internal static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length) =>
            length == 1
                ? list.Select(t => new T[] { t })
                : GetPermutations(list, length - 1)
                    .SelectMany(t => list.Where(e => !t.Contains(e)),
                        (t1, t2) => t1.Concat(new T[] { t2 }));

        internal static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> list, int length) =>
            Enumerable
                .Range(0, 1 << (list.Count()))
                .Select(index => list
                   .Where((v, i) => (index & (1 << i)) != 0))
                .Where(l => l.Count() == length);

        internal static long Product(this IEnumerable<long> list) =>
            list.Aggregate(1L, (current, item) => current * item);

        internal static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source) =>
            source.Select((item, index) => (item, index));

        internal static long LCM(this IEnumerable<long> data) =>
            data?.Any() == true ? data.Aggregate(LCM) : 0;

        internal static long GCD(long a, long b) => (b == 0) ? a : GCD(b, a % b);

        internal static long LCM(long a, long b) => a * b / GCD(a, b);
    }
}
