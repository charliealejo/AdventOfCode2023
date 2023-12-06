namespace AdventOfCode2023
{
    internal partial class D06 : Day
    {
        internal override void SolvePart1()
        {
            var rs = new (long, long)[] { (54, 239), (70, 1142), (82, 1295), (75, 1253) };
            Console.WriteLine(rs.Select(r => C(r)).Product());
        }

        internal override void SolvePart2()
        {
            (long, long) r = (54708275, 239114212951253);
            Console.WriteLine(C(r));
        }

        private static long C((long T, long D) r)
        {
            long v = 0;
            for (; v < r.T; v++)
            {
                if (v * (r.T - v) > r.D) break;
            }
            return r.T + 1 - v * 2;
        }
    }
}
