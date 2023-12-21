using System.Data;

namespace AdventOfCode2023
{
    internal class D19 : Day
    {
        internal override void SolvePart1()
        {
            var file = FileHelper.ReadAll(FileName).Split(Environment.NewLine + Environment.NewLine);
            var flows = GetFlows(file[0].Split(Environment.NewLine));
            var parts = GetParts(file[1].Split(Environment.NewLine));

            var approved = parts.Where(p => IsApproved(flows, p));

            Console.WriteLine(approved.Select(p => p.Select(m => m.Value).Sum()).Sum());
        }

        internal override void SolvePart2()
        {
            var file = FileHelper.ReadAll(FileName).Split(Environment.NewLine + Environment.NewLine);
            var flows = GetFlows(file[0].Split(Environment.NewLine));

            // Console.WriteLine(GetProbability(flows));
        }

        private static Dictionary<string, string[]> GetFlows(string[] ls)
        {
            var res = new Dictionary<string, string[]>();
            foreach (var l in ls)
            {
                var p = l.Split('{');
                var n = p[0];
                var fs = p[1][..^1].Split(',');
                res.Add(n, fs);
            }
            return res;
        }

        private static List<Dictionary<char, long>> GetParts(string[] ls)
        {
            var res = new List<Dictionary<char, long>>();
            foreach (var l in ls)
            {
                var p = new Dictionary<char, long>();
                var cs = l[1..^1].Split(',');
                foreach (var c in cs)
                {
                    var vs = c.Split('=');
                    p.Add(vs[0][0], long.Parse(vs[1]));
                }
                res.Add(p);
            }
            return res;
        }

        private static bool IsApproved(Dictionary<string, string[]> flows, Dictionary<char, long> p)
        {
            var f = "in";
            while (true)
            {
                if (f == "A") return true;
                if (f == "R") return false;
                foreach (var rule in flows[f])
                {
                    if (rule.Contains(':'))
                    {
                        var cs = rule.Split(':');
                        var condition = cs[0];

                        var m = condition[0];
                        var o = condition[1];
                        var l = long.Parse(condition[2..]);

                        if (o == '<')
                        {
                            if (p[m] < l)
                            {
                                f = cs[1];
                                break;
                            }
                        }
                        else
                        {
                            if (p[m] > l)
                            {
                                f = cs[1];
                                break;
                            }
                        }
                    }
                    else
                    {
                        f = rule;
                        break;
                    }
                }
            }
        }
    }
}
