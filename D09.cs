namespace AdventOfCode2023
{
    internal class D09 : Day
    {
        internal override void SolvePart1()
        {
            Solve(true);
        }

        internal override void SolvePart2()
        {
            Solve(false);
        }

        private void Solve(bool next)
        {
            var lines = FileHelper.ReadLinesAsIntLists(FileName, " ");
            Console.WriteLine(lines.Sum(line => GetValue(line.ToArray(), next)));
        }

        private long GetValue(long[] line, bool next)
        {
            if (line.All(x => x == 0)) return 0;

            var v = GetValue(line[1..].Select((x, i) => x - line[i]).ToArray(), next);
            return next ? line.Last() + v : line.First() - v;
        }
    }
}
