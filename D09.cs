namespace AdventOfCode2023
{
    internal class D09 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLinesAsIntLists(FileName, " ");

            long r = 0;
            foreach (var line in lines)
            {
                r += CalculateNextValue(line.ToArray());
            }

            Console.WriteLine(r);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLinesAsIntLists(FileName, " ");

            long r = 0;
            foreach (var line in lines)
            {
                r += CalculatePreviousValue(line.ToArray());
            }

            Console.WriteLine(r);
        }

        private long CalculateNextValue(long[] line)
        {
            return line.All(x => x == 0)
                   ? 0
                   : line.Last() + CalculateNextValue(line[..^1].Select((x, i) => line[i + 1] - x).ToArray());
        }

        private long CalculatePreviousValue(long[] line)
        {
            return line.All(x => x == 0)
                   ? 0
                   : line.First() + CalculatePreviousValue(line[1..].Select((x, i) => line[i] - x).ToArray());
        }
    }
}
