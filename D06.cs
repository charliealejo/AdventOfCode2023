namespace AdventOfCode2023
{
    internal partial class D06 : Day
    {
        internal override void SolvePart1()
        {
            var ts = new int[] { 54, 70, 82, 75 };
            var ds = new int[] { 239, 1142, 1295, 1253 };

            long p = 1;
            for (int i = 0; i < ts.Length; i++)
            {
                long c = 0;
                for (int v = 0; v < ts[i]; v++)
                {
                    if (v * (ts[i] - v) > ds[i]) c++;
                }
                p *= c;
            }

            Console.WriteLine(p);
        }

        internal override void SolvePart2()
        {
            long t = 54708275;
            long d = 239114212951253;

            long c = 0;
            for (int v = 0; v < t; v++)
            {
                if (v * (t - v) > d) c++;
            }

            Console.WriteLine(c);
        }
    }
}
