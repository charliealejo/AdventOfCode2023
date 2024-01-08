namespace AdventOfCode2023
{
    internal class D23 : Day
    {
        internal override void SolvePart1()
        {
            var map = FileHelper.ReadLinesAsCharMap(FileName);
            var path = GetLongestPath(map, true);
            Console.WriteLine(path.Length - 1);
        }

        internal override void SolvePart2()
        {
            //var map = FileHelper.ReadLinesAsCharMap(FileName);
            //var path = GetLongestPath(map, false);
            //Console.WriteLine(path.Length - 1);
        }

        private static (int x, int y)[] GetLongestPath(char[][] map, bool slopes)
        {
            var paths = new List<List<(int, int)>>();
            DFS(map, 0, 1, new List<(int, int)>(), paths, slopes);
            return paths.OrderBy(l => l.Count).Last().ToArray();
        }

        private static void DFS(char[][] map, int r, int c, List<(int, int)> path, List<List<(int, int)>> paths, bool slopes)
        {
            if (map[r][c] == '#') return;

            if (r == map.Length - 1 && c == map[0].Length - 2)
            {
                path.Add((r, c));
                paths.Add(new List<(int, int)>(path));
                path.RemoveAt(path.Count - 1);
                return;
            }

            var slope = ">v<^".IndexOf(map[r][c]);

            var prev = map[r][c];
            map[r][c] = '#';
            path.Add((r, c));

            var ds = new (int r, int c)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            if (slopes && slope >= 0)
            {
                DFS(map, r + ds[slope].r, c + ds[slope].c, path, paths, slopes);
            }
            else
            {
                foreach (var d in ds)
                {
                    int nr = r + d.r;
                    int nc = c + d.c;
                    if (nr >= 0 && nr < map.Length && nc >= 0 && nc < map[r].Length && map[nr][nc] != '#')
                    {
                        DFS(map, nr, nc, path, paths, slopes);
                    }
                }
            }

            map[r][c] = prev;
            path.RemoveAt(path.Count - 1);
        }
    }
}
