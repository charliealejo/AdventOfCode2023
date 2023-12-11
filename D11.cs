namespace AdventOfCode2023
{
    internal class D11 : Day
    {
        internal override void SolvePart1()
        {
            Solve(2);
        }

        internal override void SolvePart2()
        {
            Solve(1000000);
        }

        private void Solve(long expansion)
        {
            var lines = FileHelper.ReadLines(FileName).ToList();

            var (ex, ey) = Expand(lines);
            var pos = FindGalaxies(lines);
            var pairs = pos.GetPermutations(2);

            long res = 0;
            foreach (var p in pairs)
            {
                var p1x = p.First().X;
                var p1y = p.First().Y;
                var p2x = p.Last().X;
                var p2y = p.Last().Y;
                long d = Math.Abs(p1x - p2x) + Math.Abs(p1y - p2y);
                foreach (var e in ex) if (e > Math.Min(p1x, p2x) && e < Math.Max(p1x, p2x)) d += (expansion - 1);
                foreach (var e in ey) if (e > Math.Min(p1y, p2y) && e < Math.Max(p1y, p2y)) d += (expansion - 1);
                res += d;
            }

            Console.WriteLine(res / 2);
        }

        private static (List<int> X, List<int> Y) Expand(List<string> lines)
        {
            return (
                lines.Select((line, i) => line.Contains('#') ? -1 : i).Where(x => x != -1).ToList(),
                Enumerable.Range(0, lines[0].Length).Where(i => lines.All(l => l[i] == '.')).ToList()
            );
        }

        private static IEnumerable<(int X, int Y)> FindGalaxies(List<string> lines)
        {
            var res = new List<(int X, int Y)>();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#') res.Add((i, j));
                }
            }
            return res;
        }
    }
}
