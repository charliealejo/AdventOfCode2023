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
            var lines = FileHelper.ReadLines(FileName);
            var stones = GetStones(lines);
            long total = Solve(stones);

            Console.WriteLine(total);
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

        private static long Solve(Stone[] st)
        {
            int i, j, k, imax;
            double temp, f, maxi;
            long total;

            double[,] a = new double[6, 6];
            a[0, 0] = st[1].Vy - st[2].Vy;
            a[0, 1] = st[2].Vx - st[1].Vx;
            a[0, 3] = st[2].Y - st[1].Y;
            a[0, 4] = st[1].X - st[2].X;
            a[1, 0] = st[1].Vy - st[3].Vy;
            a[1, 1] = st[3].Vx - st[1].Vx;
            a[1, 3] = st[3].Y - st[1].Y;
            a[1, 4] = st[1].X - st[3].X;
            a[2, 0] = st[1].Vz - st[2].Vz;
            a[2, 2] = st[2].Vx - st[1].Vx;
            a[2, 3] = st[2].Z - st[1].Z;
            a[2, 5] = st[1].X - st[2].X;
            a[3, 0] = st[1].Vz - st[3].Vz;
            a[3, 2] = st[3].Vx - st[1].Vx;
            a[3, 3] = st[3].Z - st[1].Z;
            a[3, 5] = st[1].X - st[3].X;
            a[4, 1] = st[1].Vz - st[2].Vz;
            a[4, 2] = st[2].Vy - st[1].Vy;
            a[4, 4] = st[2].Z - st[1].Z;
            a[4, 5] = st[1].Y - st[2].Y;
            a[5, 1] = st[1].Vz - st[3].Vz;
            a[5, 2] = st[3].Vy - st[1].Vy;
            a[5, 4] = st[3].Z - st[1].Z;
            a[5, 5] = st[1].Y - st[3].Y;

            double[] c = new double[6];
            c[0] = (st[2].Y * st[2].Vx - st[2].X * st[2].Vy) - (st[1].Y * st[1].Vx - st[1].X * st[1].Vy);
            c[1] = (st[3].Y * st[3].Vx - st[3].X * st[3].Vy) - (st[1].Y * st[1].Vx - st[1].X * st[1].Vy);
            c[2] = (st[2].Z * st[2].Vx - st[2].X * st[2].Vz) - (st[1].Z * st[1].Vx - st[1].X * st[1].Vz);
            c[3] = (st[3].Z * st[3].Vx - st[3].X * st[3].Vz) - (st[1].Z * st[1].Vx - st[1].X * st[1].Vz);
            c[4] = (st[2].Z * st[2].Vy - st[2].Y * st[2].Vz) - (st[1].Z * st[1].Vy - st[1].Y * st[1].Vz);
            c[5] = (st[3].Z * st[3].Vy - st[3].Y * st[3].Vz) - (st[1].Z * st[1].Vy - st[1].Y * st[1].Vz);

            for (k = 0; k < a.GetLength(0) - 1; k++)
            {
                imax = 0;
                maxi = 0.0;

                for (i = k; i < a.GetLength(0); i++)
                {
                    temp = Math.Abs(a[i, k]);
                    if (temp > maxi)
                    {
                        maxi = temp;
                        imax = i;
                    }
                }

                temp = c[k];
                c[k] = c[imax];
                c[imax] = temp;

                for (i = 0; i < a.GetLength(0); i++)
                {
                    temp = a[k, i];
                    a[k, i] = a[imax, i];
                    a[imax, i] = temp;
                }

                for (i = k + 1; i < c.Length; i++)
                {
                    f = a[i, k] / a[k, k];
                    for (j = k; j < a.GetLength(0); j++)
                    {
                        a[i, j] -= a[k, j] * f;
                    }
                    c[i] -= c[k] * f;
                }
            }

            double[] x = new double[6];

            for (k = a.GetLength(0) - 1; k >= 0; k--)
            {
                x[k] = c[k];
                for (i = k + 1; i < a.GetLength(0); i++)
                {
                    x[k] -= a[k, i] * x[i];
                }
                x[k] /= a[k, k];
            }

            total = (long)Math.Round(x[0]) + (long)Math.Round(x[1]) + (long)Math.Round(x[2]);
            return total;
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
