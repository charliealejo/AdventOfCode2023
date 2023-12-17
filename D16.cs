namespace AdventOfCode2023
{
    internal class D16 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName).ToArray();

            var energized = GetEnergizedTiles(lines);
            Console.WriteLine(energized.Count);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName).ToArray();

            var max = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                var energized = GetEnergizedTiles(lines, i, 0, 0, 1).Count;
                if (energized > max) max = energized;
                energized = GetEnergizedTiles(lines, i, lines[0].Length - 1, 0, -1).Count;
                if (energized > max) max = energized;
            }

            for (int j = 0; j < lines[0].Length; j++)
            {
                var energized = GetEnergizedTiles(lines, 0, j, 1, 0).Count;
                if (energized > max) max = energized;
                energized = GetEnergizedTiles(lines, lines.Length - 1, j, -1, 0).Count;
                if (energized > max) max = energized;
            }

            Console.WriteLine(max);
        }

        private HashSet<(int X, int Y)> GetEnergizedTiles(string[] lines, int sx = 0, int sy = 0, int sdx = 0, int sdy = 1)
        {
            var stack = new Stack<(int X, int Y, int dx, int dy)>();
            var visited = new HashSet<(int, int, int, int)>();

            stack.Push((sx, sy, sdx, sdy));

            var set = new HashSet<(int X, int Y)>();
            while (stack.Any())
            {
                var c = stack.Pop();
                set.Add((c.X, c.Y));
                foreach (var n in GetNext(lines, c))
                {
                    if (!visited.Contains(n))
                    {
                        visited.Add(n);
                        stack.Push(n);
                    }
                }
            }

            return set;
        }

        private IEnumerable<(int X, int Y, int dx, int dy)> GetNext(string[] lines, (int x, int y, int dx, int dy) c)
        {
            var candidates = new List<(int X, int Y, int dx, int dy)>();

            switch (lines[c.x][c.y])
            {
                case '.':
                    candidates.Add((c.x + c.dx, c.y + c.dy, c.dx, c.dy));
                    break;
                case '/':
                    candidates.Add((c.x - c.dy, c.y - c.dx, -c.dy, -c.dx));
                    break;
                case '\\':
                    candidates.Add((c.x + c.dy, c.y + c.dx, c.dy, c.dx));
                    break;
                case '|':
                    if (c.dy == 0)
                    {
                        candidates.Add((c.x + c.dx, c.y + c.dy, c.dx, c.dy));
                    }
                    else
                    {
                        candidates.Add((c.x - c.dy, c.y - c.dx, -c.dy, -c.dx));
                        candidates.Add((c.x + c.dy, c.y + c.dx, c.dy, c.dx));
                    }

                    break;
                case '-':
                    if (c.dx == 0)
                    {
                        candidates.Add((c.x + c.dx, c.y + c.dy, c.dx, c.dy));
                    }
                    else
                    {
                        candidates.Add((c.x - c.dy, c.y - c.dx, -c.dy, -c.dx));
                        candidates.Add((c.x + c.dy, c.y + c.dx, c.dy, c.dx));
                    }

                    break;
                default:
                    break;
            }

            return RemoveCandidatesOutside(lines.Length, lines[0].Length, candidates);
        }

        private List<(int X, int Y, int dx, int dy)> RemoveCandidatesOutside(int lx, int ly,
            List<(int X, int Y, int dx, int dy)> list)
        {
            return list.Where(c => c.X >= 0 && c.X < lx && c.Y >= 0 && c.Y < ly).ToList();
        }
    }
}
