namespace AdventOfCode2023
{
    internal class D01 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);

            int r = 0;
            foreach (var line in lines)
            {
                var d1 = line.First(char.IsDigit);
                var d2 = line.Last(char.IsDigit);
                var ns = "" + d1 + d2;
                var n = int.Parse(ns);
                r += n;
            }

            Console.WriteLine(r);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName);

            int r = 0;
            foreach (var line in lines)
            {
                var d1 = _digits.Select(d => line.IndexOf(d)).Where(v => v >= 0).Min();
                var d2 = _digits.Select(d => line.LastIndexOf(d)).Where(v => v >= 0).Max();

                var n1 = char.IsDigit(line[d1]) ? int.Parse("" + line[d1]) : ParseDigit(line[d1..]);
                var n2 = char.IsDigit(line[d2]) ? int.Parse("" + line[d2]) : ParseDigit(line[d2..]);

                var n = n1 * 10 + n2;
                r += n;
            }

            Console.WriteLine(r);
        }

        private int ParseDigit(string v)
        {
            for (int i = 0; i < 10; i++)
            {
                if (v.StartsWith(_digits[i])) return i + 1;
            }

            return 0;
        }

        private readonly string[] _digits = new string[]
        {
            "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
            "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };
    }
}
