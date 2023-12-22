namespace AdventOfCode2023
{
    internal class D21 : Day
    {
        internal override void SolvePart1()
        {
            var map = FileHelper.ReadLinesAsCharMap(FileName);
            Console.WriteLine(GetDistinctDestinations(map, 64));
        }

        internal override void SolvePart2()
        {
            var map = FileHelper.ReadLinesAsCharMap(FileName);
            Console.WriteLine(GetDistinctDestinationsWithInfiniteMap(map, 26501365));
        }

        private static long GetDistinctDestinations(char[][] map, int steps)
        {
            (int r, int c) = map.WithIndex().Where(l => l.item.Contains('S')).Select(l => (l.index, l.item.WithIndex().First(c => c.item == 'S').index)).First();
            map[r][c] = '.';

            var ds = new (int r, int c)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            var current = new HashSet<(int r, int c)> { (r, c) };
            var next = new HashSet<(int r, int c)>();
            for (int i = 0; i < steps; i++)
            {
                foreach (var p in current)
                {
                    foreach (var d in ds)
                    {
                        var n = (p.c + d.c, p.r + d.r);
                        if (map[n.Item1][n.Item2] != '#') next.Add(n);
                    }
                }
                current = next.Where(p => p.r >= 0 && p.r < map.Length && p.c >= 0 && p.c < map[0].Length).ToHashSet();
                next.Clear();
            }

            return current.LongCount();
        }

        /// <remarks>
        /// After some data analysis, the plot of the differences between each step
        /// follows a somewhat sinusoidal wave with a period of 131 (the data map
        /// lengths) and an ever increasing pace. We can use that to calculate the
        /// number required
        /// </remarks>
        private static long GetDistinctDestinationsWithInfiniteMap(char[][] map, int steps)
        {
            (int r, int c) = map.WithIndex().Where(l => l.item.Contains('S')).Select(l => (l.index, l.item.WithIndex().First(c => c.item == 'S').index)).First();
            map[r][c] = '.';

            var ds = new (int r, int c)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            var current = new HashSet<(int r, int c)> { (r, c) };
            var next = new HashSet<(int r, int c)>();
            var diffs = new long[map.Length, 2];
            for (int i = 0; i < map.Length * 2; i++)
            {
                foreach (var p in current)
                {
                    foreach (var d in ds)
                    {
                        var n = (p.c + d.c, p.r + d.r);
                        if (map[(n.Item1 % map.Length + map.Length) % map.Length][(n.Item2 % map[0].Length + map[0].Length) % map[0].Length] != '#') next.Add(n);
                    }
                }

                diffs[i % map.Length, i / map.Length] = next.Count - current.Count;

                current = new HashSet<(int r, int c)>(next);
                next.Clear();
            }

            long count = 1;
            for (int i = 0; i < steps; i++)
            {
                count += diffs[i % map.Length, 0] + (diffs[i % map.Length, 1] - diffs[i % map.Length, 0]) * (i / map.Length);
            }

            return count;
        }
    }
}
