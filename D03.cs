using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal class D03 : Day
    {
        private IEnumerable<Item> _map;

        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            _map = GetItemMap(lines);

            long r = 0;
            foreach (var item in _map.Where(i => !i.S))
            {
                if (IsAdjacentToSymbol(item))
                {
                    r += item.N;
                }
            }

            Console.WriteLine(r);
        }

        internal override void SolvePart2()
        {
            long r = 0;

            foreach (var item in _map.Where(i => i.S && i.N == '*'))
            {
                var ns = GetAdjacentNumbers(item);
                if (ns.Count == 2)
                {
                    r += ns.First() * ns.Last();
                }
            }

            Console.WriteLine(r);
        }

        private static IEnumerable<Item> GetItemMap(IEnumerable<string> lines)
        {
            var y = 0;
            var reg = new Regex("[0-9]+");

            var l = new List<Item>();
            foreach (var line in lines)
            {
                var matches = reg.Matches(line);
                foreach (var x in matches.ToList())
                {
                    l.Add(new Item { N = int.Parse(x.Value), L = x.Value.Length, X = x.Index, Y = y, S = false });
                }
                for (var x = 0; x < line.Length; x++)
                {
                    if (!char.IsDigit(line[x]) && line[x] != '.')
                    {
                        l.Add(new Item { N = line[x], X = x, Y = y, S = true });
                    }
                }
                y++;
            }

            return l;
        }

        private bool IsAdjacentToSymbol(Item n)
        {
            foreach (var s in _map.Where(i => i.S))
            {
                if (ExistsOverlap(n, s))
                {
                    return true;
                }
            }

            return false;
        }

        private ICollection<int> GetAdjacentNumbers(Item s)
        {
            var l = new List<int>();

            foreach (var n in _map.Where(i => !i.S))
            {
                if (ExistsOverlap(n, s))
                {
                    l.Add(n.N);
                }
            }

            return l;
        }

        private static bool ExistsOverlap(Item n, Item s)
        {
            return s.X >= n.X - 1 && s.X <= n.X + n.L && s.Y >= n.Y - 1 && s.Y <= n.Y + 1;
        }

        internal class Item
        {
            internal int N { get; set; }

            internal int X { get; set; }

            internal int Y { get; set; }

            internal int L { get; set; }

            internal bool S { get; set; }
        }
    }
}
