using System.Text;

namespace AdventOfCode2023
{
    internal class D14 : Day
    {
        internal override void SolvePart1()
        {
            var map = FileHelper.ReadLines(FileName).Select(l => l.ToCharArray()).ToArray();
            map.TiltNorth();
            Console.WriteLine(map.WeightOnNorth());
        }

        internal override void SolvePart2()
        {
            var map = FileHelper.ReadLines(FileName).Select(l => l.ToCharArray()).ToArray();
            var d = new Dictionary<string, int>();
            var l = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                map.TiltNorth();
                map.TiltWest();
                map.TiltSouth();
                map.TiltEast();

                if (l == 0)
                {
                    var w = map.GetKey();
                    if (d.ContainsKey(w))
                    {
                        l = i - d[w];
                        var s = (1000000000 - i) % l;
                        i = 1000000000 - s;
                    }
                    else
                    {
                        d.Add(w, i);
                    }
                }
            }
            Console.WriteLine(map.WeightOnNorth());
        }
    }

    internal static class E
    {
        internal static void TiltNorth(this char[][] map)
        {
            for (var i = 1; i < map.Length; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'O')
                    {
                        map.PushNorth(i, j);
                    }
                }
            }
        }

        internal static void PushNorth(this char[][] map, int i, int j)
        {
            if (i == 0) return;
            if (map[i - 1][j] == '.')
            {
                map[i - 1][j] = 'O';
                map[i][j] = '.';
                map.PushNorth(i - 1, j);
            }
        }

        internal static void TiltSouth(this char[][] map)
        {
            for (var i = map.Length - 1; i >= 0; i--)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'O')
                    {
                        map.PushSouth(i, j);
                    }
                }
            }
        }

        internal static void PushSouth(this char[][] map, int i, int j)
        {
            if (i == map.Length - 1) return;
            if (map[i + 1][j] == '.')
            {
                map[i + 1][j] = 'O';
                map[i][j] = '.';
                map.PushSouth(i + 1, j);
            }
        }

        internal static void TiltWest(this char[][] map)
        {
            for (var i = 0; i < map.Length; i++)
            {
                for (var j = 1; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'O')
                    {
                        map.PushWest(i, j);
                    }
                }
            }
        }

        internal static void PushWest(this char[][] map, int i, int j)
        {
            if (j == 0) return;
            if (map[i][j - 1] == '.')
            {
                map[i][j - 1] = 'O';
                map[i][j] = '.';
                map.PushWest(i, j - 1);
            }
        }

        internal static void TiltEast(this char[][] map)
        {
            for (var i = 0; i < map.Length; i++)
            {
                for (var j = map[i].Length - 1; j >= 0; j--)
                {
                    if (map[i][j] == 'O')
                    {
                        map.PushEast(i, j);
                    }
                }
            }
        }

        internal static void PushEast(this char[][] map, int i, int j)
        {
            if (j == map[i].Length - 1) return;
            if (map[i][j + 1] == '.')
            {
                map[i][j + 1] = 'O';
                map[i][j] = '.';
                map.PushEast(i, j + 1);
            }
        }

        internal static long WeightOnNorth(this char[][] map)
        {
            long res = 0;

            for (var i = 0; i < map.Length; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'O')
                    {
                        res += map.Length - i;
                    }
                }
            }

            return res;
        }

        internal static string GetKey(this char[][] map)
        {
            var key = new StringBuilder();

            for (var i = 0; i < map.Length; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    key.Append(map[i][j]);
                }
            }

            return key.ToString();
        }
    }
}
