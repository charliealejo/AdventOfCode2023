namespace AdventOfCode2023
{
    internal class D12 : Day
    {
        private static readonly Dictionary<(int, int, int), long> dp = new();

        private string map;
        private int[] blocks;

        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            var springs = lines.Select(l => l.Split(' ')[0]).ToArray();
            var data = lines.Select(l => l.Split(' ')[1].Split(',').Select(int.Parse).ToArray()).ToArray();

            long result = 0;
            for (int i = 0; i < springs.Length; i++)
            {
                map = springs[i];
                blocks = data[i];

                dp.Clear();
                var c = CalculateCombinations(0, 0, 0);

                result += c;
            }

            Console.WriteLine(result);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName);
            var springs = lines.Select(l => l.Split(' ')[0]).Select(l => l + "?" + l + "?" + l + "?" + l + "?" + l).ToArray();
            var data = lines.Select(l => l.Split(' ')[1].Split(',').Select(int.Parse).ToArray()).ToArray();

            long result = 0;
            for (int i = 0; i < springs.Length; i++)
            {
                var s = springs[i];
                var d = new List<int>();
                d.AddRange(data[i]);
                d.AddRange(data[i]);
                d.AddRange(data[i]);
                d.AddRange(data[i]);
                d.AddRange(data[i]);

                map = springs[i];
                blocks = d.ToArray();

                dp.Clear();
                var c = CalculateCombinations(0, 0, 0);

                result += c;
            }

            Console.WriteLine(result);
        }

        /// <summary>
        /// Calculates the number of valid combinations of the map
        /// </summary>
        /// <param name="i">Position within map</param>
        /// <param name="bi">Position within blocks</param>
        /// <param name="current">Length of current block of '#'</param>
        private long CalculateCombinations(int i, int bi, int current)
        {
            var key = (i, bi, current);
            if (dp.ContainsKey(key))
            {
                return dp[key];
            }

            if (i == map.Length)
            {
                return (bi == blocks.Length && current == 0)
                    || (bi == blocks.Length - 1 && blocks[bi] == current)
                    ? 1
                    : 0;
            }

            long ans = 0;
            foreach (char c in new[] { '.', '#' })
            {
                if (map[i] == c || map[i] == '?')
                {
                    if (c == '.' && current == 0)
                    {
                        ans += CalculateCombinations(i + 1, bi, 0);
                    }
                    else if (c == '.' && current > 0 && bi < blocks.Length && blocks[bi] == current)
                    {
                        ans += CalculateCombinations(i + 1, bi + 1, 0);
                    }
                    else if (c == '#')
                    {
                        ans += CalculateCombinations(i + 1, bi, current + 1);
                    }
                }
            }

            dp[key] = ans;
            return ans;
        }
    }
}
