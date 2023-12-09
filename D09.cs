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
            var res = lines.Sum(line => GetValue(next ? line.ToArray() : line.Reverse().ToArray()));
            Console.WriteLine(res);
        }

        private long GetValue(long[] line)
        {
            return line.All(x => x == 0)
                   ? 0
                   : line.Last() + GetValue(line[1..].Select((x, i) => x - line[i]).ToArray());
        }
    }
}
