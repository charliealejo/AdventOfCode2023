namespace AdventOfCode2023
{
    internal class D23 : Day
    {
        private static (int r, int c)[] _ds = new (int r, int c)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        private static HashSet<(int, int)> _path;
        private static List<List<(int, int)>> _paths;
        private static char[][] _map;

        internal override void SolvePart1()
        {
            _map = FileHelper.ReadLinesAsCharMap(FileName);
            var path = GetLongestPath(true);
            Console.WriteLine(path.Length - 1);
        }

        internal override void SolvePart2()
        {
            // Brute force, takes several minutes
            _map = FileHelper.ReadLinesAsCharMap(FileName);
            var path = GetLongestPath(false);
            Console.WriteLine(path.Length - 1);
        }

        private static (int x, int y)[] GetLongestPath(bool slopes)
        {
            _path = new HashSet<(int, int)>();
            _paths = new List<List<(int, int)>>();
            DFS(0, 1, slopes);
            return _paths.OrderBy(l => l.Count).Last().ToArray();
        }

        private static void DFS(int r, int c, bool slopes)
        {
            if (_map[r][c] == '#') return;

            var p = (r, c);
            if (r == _map.Length - 1 && c == _map[0].Length - 2)
            {
                _path.Add(p);
                _paths.Add(new List<(int, int)>(_path));
                _path.Remove(p);
                return;
            }

            var slope = ">v<^".IndexOf(_map[r][c]);

            var prev = _map[r][c];
            _map[r][c] = '#';
            _path.Add(p);

            if (slopes && slope >= 0)
            {
                DFS(r + _ds[slope].r, c + _ds[slope].c, slopes);
            }
            else
            {
                foreach (var d in _ds)
                {
                    int nr = r + d.r;
                    int nc = c + d.c;
                    if (nr >= 0 && nr < _map.Length && nc >= 0 && nc < _map[r].Length
                        && _map[nr][nc] != '#' && !_path.Contains((nr, nc)))
                    {
                        DFS(nr, nc, slopes);
                    }
                }
            }

            _map[r][c] = prev;
            _path.Remove(p);
        }
    }
}
