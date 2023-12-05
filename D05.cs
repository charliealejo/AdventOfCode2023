using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal partial class D05 : Day
    {
        private Almanac _almanac;

        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            _almanac = GetAlmanac(lines);

            Console.WriteLine(_almanac.Seeds.Min(s => GetLocation(s)));
        }

        internal override void SolvePart2()
        {
            var seeds = new List<Range>();
            for (int i = 0; i < _almanac.Seeds.Count; i += 2)
            {
                seeds.Add(new Range { S = _almanac.Seeds[i], L = _almanac.Seeds[i + 1] });
            }

            Console.WriteLine(
                seeds.Select(r => GetLocationRanges(r, 0).OrderBy(r => r.S).First().S).Min());
        }

        private Almanac GetAlmanac(IEnumerable<string> lines)
        {
            var maps = new Almanac();

            List<Range> ranges = null;
            foreach (var line in lines)
            {
                var ns = Numbers().Matches(line);
                if (ns.Count > 3)
                {
                    maps.Seeds = ns.Select(m => long.Parse(m.Value)).ToList();
                }
                else if (!string.IsNullOrEmpty(line) && ns.Count == 0)
                {
                    if (ranges != null)
                    {
                        maps.Mappings.Add(ranges.OrderBy(r => r.S).ToList());
                    }

                    ranges = new List<Range>();
                }
                else if (ns.Count == 3)
                {
                    ranges.Add(
                        new Range
                        {
                            S = long.Parse(ns[1].Value),
                            D = long.Parse(ns[0].Value),
                            L = long.Parse(ns[2].Value)
                        });
                }
            }
            if (ranges?.Any() == true)
            {
                maps.Mappings.Add(ranges.OrderBy(r => r.S).ToList());
            }

            return maps;
        }

        private long GetLocation(long s)
        {
            foreach (var map in _almanac.Mappings)
            {
                s = Map(map, s);
            }

            return s;
        }

        private List<Range> GetLocationRanges(Range r, int d)
        {
            var ranges = new List<Range>();

            var map = _almanac.Mappings[d];
            for (var i = 0; i < map.Count; i++)
            {
                var mr = map[i];

                if (mr.S + mr.L < r.S) continue;
                if (r.L <= 0 || mr.S >= r.S + r.L) break;

                if (r.S < mr.S)
                {
                    ranges.Add(new Range { S = r.S, L = mr.S - r.S });
                    r.L -= mr.S - r.S;
                    r.S = mr.S;
                }

                ranges.Add(new Range { S = mr.D + (r.S - mr.S), L = Math.Min(r.L, mr.S + mr.L - r.S) });
                r.L -= mr.S + mr.L - r.S;
                r.S = mr.S + mr.L;
            }

            if (r.L > 0)
            {
                ranges.Add(r);
            }

            if (d < _almanac.Mappings.Count - 1)
            {
                ranges = ranges.SelectMany(r => GetLocationRanges(r, d + 1)).ToList();
            }

            return ranges;
        }

        private static long Map(List<Range> rs, long n)
        {
            foreach (var r in rs)
            {
                if (n >= r.S && n < r.S + r.L)
                {
                    return r.D + (n - r.S);
                }
            }

            return n;
        }

        [GeneratedRegex("[0-9]+")]
        private static partial Regex Numbers();
    }

    internal class Almanac
    {
        internal List<long> Seeds { get; set; }

        internal List<List<Range>> Mappings { get; set; } = new List<List<Range>>();
    }

    internal struct Range
    {
        internal long S { get; set; }

        internal long D { get; set; }

        internal long L { get; set; }
    }
}
