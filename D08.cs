namespace AdventOfCode2023
{
    internal class D08 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName).ToArray();
            var directions = lines[0];
            var map = GetInstructions(lines[2..]);

            var i = 0;
            var current = "AAA";
            var end = "ZZZ";
            while (current != end)
            {
                current = directions[i % directions.Length] == 'R' ? map[current].R : map[current].L;
                i++;
            }

            Console.WriteLine(i);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName).ToArray();
            var directions = lines[0];
            var map = GetInstructions(lines[2..]);

            var current = map.Keys.Where(k => k.EndsWith("A")).ToArray();
            var times = current.Select(c =>
            {
                var i = 0;
                while (!c.EndsWith("Z"))
                {
                    c = directions[i % directions.Length] == 'R' ? map[c].R : map[c].L;
                    i++;
                }
                return (long)i;
            });

            Console.WriteLine(times.LCM());
        }

        private static Dictionary<string, (string L, string R)> GetInstructions(IEnumerable<string> lines)
        {
            var d = new Dictionary<string, (string, string)>();
            foreach (var line in lines)
            {
                var parts = line.Split(" = ");
                var nodes = parts[1][1..^1].Split(", ");
                d.Add(parts[0], (nodes[0], nodes[1]));
            }
            return d;
        }
    }
}
