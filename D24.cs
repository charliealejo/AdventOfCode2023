namespace AdventOfCode2023
{
    internal class D24 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            var stones = GetStones(lines);
            Console.WriteLine(GetIntersections(stones));
        }

        internal override void SolvePart2()
        {

        }

        private static Stone[] GetStones(IEnumerable<string> lines)
        {
            var stones = new List<Stone>();

            int id = 1;
            foreach (var line in lines)
            {
                var parts = line.Split(" @ ");
                var pos = parts[0].Split(", ");
                var vel = parts[1].Split(", ");
                stones.Add(new Stone
                {
                    Id = id++,
                    X = long.Parse(pos[0]),
                    Y = long.Parse(pos[1]),
                    Z = long.Parse(pos[2]),
                    Vx = long.Parse(vel[0]),
                    Vy = long.Parse(vel[1]),
                    Vz = long.Parse(vel[2])
                });
            }

            return stones.ToArray();
        }

        private static int GetIntersections(Stone[] stones)
        {
            var intersections = 0;

            for (int i = 0; i < stones.Length - 1; i++)
            {
                for (var j = i + 1; j < stones.Length; j++)
                {
                    var s1 = stones[i];
                    var s2 = stones[j];

                    var d = s2.Vy - s2.Vx * s1.Vy / (double)s1.Vx;
                    if (d == 0) continue;

                    var t2 = (s1.Y - s2.Y + (s2.X - s1.X) * s1.Vy / s1.Vx) / d;
                    var t1 = (s2.X - s1.X + t2 * s2.Vx) / s1.Vx;

                    if (t1 >= 0 && t2 >= 0)
                    {
                        var x = s1.X + t1 * s1.Vx;
                        var y = s1.Y + t1 * s1.Vy;

                        if (x is >= 200000000000000 and <= 400000000000000 &&
                            y is >= 200000000000000 and <= 400000000000000)
                        {
                            intersections++;
                        }
                    }
                }
            }

            return intersections;
        }

        internal class Stone
        {
            public int Id { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
            public long Vx { get; set; }
            public long Vy { get; set; }
            public long Vz { get; set; }
        }
    }
}
