namespace AdventOfCode2023
{
    internal class D18 : Day
    {
        private readonly (int x, int y)[] ds = new (int x, int y)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName).Select(l => l.Split(' ')).ToArray();

            var p = (0, 0);
            var plan = new List<(long x, long y)> { p };
            var per = 0;
            foreach (var l in lines)
            {
                var d = l[0][0] switch
                {
                    'R' => 0,
                    'L' => 2,
                    'U' => 3,
                    'D' => 1,
                    _ => throw new NotImplementedException()
                };
                var a = int.Parse(l[1]);
                p = (p.Item1 + ds[d].x * a, p.Item2 + ds[d].y * a);
                plan.Add(p);
                per += a;
            }

            Console.WriteLine(GetArea(plan, per));
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName).Select(l => l.Split(' ')).ToArray();

            var p = (0L, 0L);
            var plan = new List<(long x, long y)> { p };
            var per = 0L;
            foreach (var l in lines)
            {
                var d = int.Parse("" + l[2][7]);
                var a = Convert.ToInt64(l[2][2..7], 16);
                p = (p.Item1 + ds[d].x * a, p.Item2 + ds[d].y * a);
                plan.Add(p);
                per += a;
            }

            Console.WriteLine(GetArea(plan, per));
        }

        /// <summary>
        /// Sum of partial areas by triangulation: position of X-coordinate of each point, times
        /// the difference between the Y-coordinates of the next and previous points, divided by 2.
        /// After that we need to fix the area by including half (plus 1) of the squares in the perimeter.
        /// </summary>
        private static long GetArea(List<(long x, long y)> plan, long per)
        {
            var area = Math.Abs(plan.Select((p, i) =>
                p.x * (plan[(i + plan.Count - 1) % plan.Count].y - plan[(i + 1) % plan.Count].y)).Sum()) / 2;
            return area + per / 2 + 1;
        }
    }
}
