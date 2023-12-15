namespace AdventOfCode2023
{
    internal class D13 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            var maps = CreateMaps(lines);

            long res = 0;

            foreach (var map in maps)
            {
                res += CalculateSymmetry(map);
            }

            Console.WriteLine(res);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName);
            var maps = CreateMaps(lines);

            long res = 0;

            foreach (var map in maps)
            {
                var score = CalculateSymmetry(map);
                var found = false;
                for (int i = 0; i < map.Length; i++)
                {
                    for (int j = 0; j < map[i].Length; j++)
                    {
                        var c = map[i][j];
                        map[i] = map[i][..j] + (map[i][j] == '.' ? '#' : '.') + map[i][(j + 1)..];
                        var s = CalculateSymmetry(map, score);
                        if (s != 0 && s != score)
                        {
                            res += s;
                            found = true;
                            break;
                        }
                        map[i] = map[i][..j] + c + map[i][(j + 1)..];
                    }
                    if (found) break;
                }
                if (!found) { throw new Exception(); }
            }

            Console.WriteLine(res);
        }

        private static string[][] CreateMaps(IEnumerable<string> lines)
        {
            var maps = new List<string[]>();
            var map = new List<string>();
            foreach (var l in lines)
            {
                if (string.IsNullOrEmpty(l))
                {
                    maps.Add(map.ToArray());
                    map = new List<string>();
                }
                else
                {
                    map.Add(l);
                }
            }
            maps.Add(map.ToArray());
            return maps.ToArray();
        }

        private static long CalculateSymmetry(string[] map, long prev = 0)
        {
            for (int i = 1; i < map.Length; i++)
            {
                var h = map[..i];
                var t = map[i..];
                if (h.Length > t.Length) h = h.Skip(h.Length - t.Length).ToArray();
                else t = t.Take(h.Length).ToArray();
                h = h.Reverse().ToArray();

                if (h.WithIndex().All(l => l.item == t[l.index]) && (100 * i != prev)) return 100 * i;
            }

            var mapp = new string[map[0].Length];
            foreach (var (_, i) in map[0].WithIndex())
            {
                mapp[i] = new string(map.Select(l => l[i]).ToArray());
            }

            for (int i = 1; i < mapp.Length; i++)
            {
                var h = mapp[..i];
                var t = mapp[i..];
                if (h.Length > t.Length) h = h.Skip(h.Length - t.Length).ToArray();
                else t = t.Take(h.Length).ToArray();
                h = h.Reverse().ToArray();

                if (h.WithIndex().All(l => l.item == t[l.index]) && i != prev) return i;
            }

            return 0;
        }
    }
}
