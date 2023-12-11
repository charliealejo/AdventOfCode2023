namespace AdventOfCode2023
{
    internal class D10 : Day
    {
        internal override void SolvePart1()
        {
            var map = FileHelper.ReadLines(FileName).ToArray();
            var loop = GetLoop(ref map);

            Console.WriteLine(loop.Count() / 2);
        }

        internal override void SolvePart2()
        {
            var map = FileHelper.ReadLines(FileName).ToArray();
            var loop = GetLoop(ref map);
            int res = CalculateAreaInside(map, new HashSet<(int X, int Y)>(loop));

            Console.WriteLine(res);
        }

        private static IEnumerable<(int X, int Y)> GetLoop(ref string[] map)
        {
            var pos = GetStartingPosition(map);

            var res = new List<(int X, int Y)> { pos };
            var cur = pos;
            var d = D.S;
            do
            {
                if (d is D.S or D.U
                    && cur.X > 0
                    && "F|7".Contains(map[cur.X - 1][cur.Y])
                    && !res.Contains((cur.X - 1, cur.Y)))
                {
                    cur = (cur.X - 1, cur.Y);
                    d = map[cur.X][cur.Y] switch
                    {
                        '|' => D.U,
                        'F' => D.R,
                        '7' => D.L,
                        _ => throw new NotImplementedException()
                    };
                    res.Add(cur);
                }
                else if (d is D.S or D.D
                    && cur.X < map.Length - 1
                    && "L|J".Contains(map[cur.X + 1][cur.Y])
                    && !res.Contains((cur.X + 1, cur.Y)))
                {
                    cur = (cur.X + 1, cur.Y);
                    d = map[cur.X][cur.Y] switch
                    {
                        '|' => D.D,
                        'L' => D.R,
                        'J' => D.L,
                        _ => throw new NotImplementedException()
                    };
                    res.Add(cur);
                }
                else if (d is D.S or D.L
                    && cur.Y > 0
                    && "F-L".Contains(map[cur.X][cur.Y - 1])
                    && !res.Contains((cur.X, cur.Y - 1)))
                {
                    cur = (cur.X, cur.Y - 1);
                    d = map[cur.X][cur.Y] switch
                    {
                        '-' => D.L,
                        'L' => D.U,
                        'F' => D.D,
                        _ => throw new NotImplementedException()
                    };
                    res.Add(cur);
                }
                else if (d is D.S or D.R
                    && cur.Y < map[cur.X].Length - 1
                    && "7-J".Contains(map[cur.X][cur.Y + 1])
                    && !res.Contains((cur.X, cur.Y + 1)))
                {
                    cur = (cur.X, cur.Y + 1);
                    d = map[cur.X][cur.Y] switch
                    {
                        '-' => D.R,
                        '7' => D.D,
                        'J' => D.U,
                        _ => throw new NotImplementedException()
                    };
                    res.Add(cur);
                }
                else if ((cur.X > 0 && map[cur.X - 1][cur.Y] == 'S')
                      || (cur.Y > 0 && map[cur.X][cur.Y - 1] == 'S')
                      || (cur.X < map.Length - 1 && map[cur.X + 1][cur.Y] == 'S')
                      || (cur.Y < map[cur.X].Length - 1 && map[cur.X][cur.Y + 1] == 'S')) break;
            }
            while (true);

            if (d == D.U && "-7J".Contains(map[cur.X - 1][cur.Y + 1])) map[cur.X - 1] = map[cur.X - 1].Replace('S', 'F');
            else if (d == D.U && "-LF".Contains(map[cur.X - 1][cur.Y - 1])) map[cur.X - 1] = map[cur.X - 1].Replace('S', '7');
            else if (d == D.D && "-7J".Contains(map[cur.X + 1][cur.Y + 1])) map[cur.X + 1] = map[cur.X + 1].Replace('S', 'L');
            else if (d == D.D && "-LF".Contains(map[cur.X + 1][cur.Y - 1])) map[cur.X + 1] = map[cur.X + 1].Replace('S', 'J');
            else if (d == D.L && "|F7".Contains(map[cur.X - 1][cur.Y - 1])) map[cur.X] = map[cur.X].Replace('S', 'L');
            else if (d == D.L && "|JL".Contains(map[cur.X + 1][cur.Y - 1])) map[cur.X] = map[cur.X].Replace('S', 'F');
            else if (d == D.R && "|F7".Contains(map[cur.X - 1][cur.Y + 1])) map[cur.X] = map[cur.X].Replace('S', 'J');
            else if (d == D.R && "|JL".Contains(map[cur.X + 1][cur.Y + 1])) map[cur.X] = map[cur.X].Replace('S', '7');

            return res;
        }

        private static (int X, int Y) GetStartingPosition(string[] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].Contains('S')) return (i, map[i].IndexOf('S'));
            }
            return (-1, -1);
        }

        private static int CalculateAreaInside(string[] map, HashSet<(int X, int Y)> loop)
        {
            var res = 0;
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (loop.Contains((i, j))) continue;
                    int u = 0;
                    char lastBend = '.';
                    for (int e = i - 1; e >= 0; e--)
                    {
                        if (loop.Contains((e, j)))
                        {
                            if (map[e][j] == '-') u++;
                            else if ("F7LJ".Contains(map[e][j]))
                            {
                                if (lastBend == '.') lastBend = map[e][j];
                                else
                                {
                                    if (lastBend == 'L' && map[e][j] == '7') u++;
                                    else if (lastBend == 'J' && map[e][j] == 'F') u++;
                                    if (map[e][j] != '|') lastBend = '.';
                                }
                            }
                        }
                    }
                    if (u % 2 == 1) { res++; }
                }
            }

            return res;
        }

        private enum D { U, D, L, R, S }
    }
}
