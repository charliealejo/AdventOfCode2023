namespace AdventOfCode2023
{
    internal class D22 : Day
    {
        private int count;

        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            var pieces = GetPieces(lines);
            pieces = FallDown(pieces);
            var r = GetRemovablePieces(pieces);
            Console.WriteLine(r);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName);
            var pieces = GetPieces(lines);
            pieces = FallDown(pieces);
            count = 0;
            foreach (var p in pieces)
            {
                FallDown(pieces.Except(new[] { p }).ToArray());
            }
            Console.WriteLine(count);
        }

        private static (int x1, int y1, int z1, int x2, int y2, int z2)[] GetPieces(IEnumerable<string> lines)
        {
            var pieces = new List<(int x1, int y1, int z1, int x2, int y2, int z2)>();
            foreach (var line in lines)
            {
                var cs = line.Split('~');
                var p1 = cs[0].Split(',').Select(int.Parse).ToArray();
                var p2 = cs[1].Split(',').Select(int.Parse).ToArray();
                pieces.Add((p1[0], p1[1], p1[2], p2[0], p2[1], p2[2]));
            }
            return pieces.OrderBy(p => p.z1).ToArray();
        }

        private (int x1, int y1, int z1, int x2, int y2, int z2)[] FallDown(
            (int x1, int y1, int z1, int x2, int y2, int z2)[] pieces)
        {
            var floor = new int[pieces.Max(p => p.x2 + 1), pieces.Max(p => p.y2 + 1)];

            var res = new List<(int x1, int y1, int z1, int x2, int y2, int z2)>();
            foreach (var p in pieces)
            {
                var blocks = new HashSet<(int x, int y)>();
                for (int i = p.x1; i <= p.x2; i++)
                {
                    for (int j = p.y1; j <= p.y2; j++)
                    {
                        blocks.Add((i, j));
                    }
                }
                var minh = blocks.Max(b => floor[b.x, b.y]) + 1;

                var dz = p.z1 - minh;
                if (dz > 0)
                {
                    count++;
                }
                res.Add((p.x1, p.y1, minh, p.x2, p.y2, p.z2 - dz));
                foreach (var b in blocks)
                {
                    floor[b.x, b.y] = p.z2 - dz;
                }
            }

            return res.ToArray();
        }

        private static long GetRemovablePieces((int x1, int y1, int z1, int x2, int y2, int z2)[] pieces)
        {
            var removables = new HashSet<int>();
            var notRemovables = new HashSet<int>();
            foreach (var (p, index) in pieces.WithIndex())
            {
                // If there are no pieces over
                var blocks = new HashSet<(int x, int y, int z)>();
                for (int i = p.x1; i <= p.x2; i++)
                {
                    for (int j = p.y1; j <= p.y2; j++)
                    {
                        blocks.Add((i, j, p.z2 + 1));
                    }
                }
                var over = blocks.Select(b => PieceAt(pieces, b)).Where(b => b != -1).Any();
                if (!over)
                {
                    removables.Add(index);
                }

                // If there are more than one piece under
                blocks.Clear();
                if (p.z1 > 1)
                {
                    for (int i = p.x1; i <= p.x2; i++)
                    {
                        for (int j = p.y1; j <= p.y2; j++)
                        {
                            blocks.Add((i, j, p.z1 - 1));
                        }
                    }
                    var under = blocks.Select(b => PieceAt(pieces, b)).Where(b => b != -1).Distinct();
                    if (under.Count() == 1)
                    {
                        notRemovables.Add(under.First());
                    }
                    if (under.Count() > 1)
                    {
                        foreach (var piece in under) removables.Add(piece);
                    }
                }
            }

            return removables.Except(notRemovables).Count();
        }

        private static int PieceAt((int x1, int y1, int z1, int x2, int y2, int z2)[] pieces, (int x, int y, int z) b)
        {
            var (p, index) = pieces.WithIndex().FirstOrDefault(p =>
                        b.x >= p.item.x1 && b.x <= p.item.x2
                     && b.y >= p.item.y1 && b.y <= p.item.y2
                     && b.z >= p.item.z1 && b.z <= p.item.z2);
            return p.z1 == 0 ? -1 : index;
        }
    }
}
