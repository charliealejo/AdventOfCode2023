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
            var ranges = "xmas".ToDictionary(c => c, c => (1L, 4000L));

            Console.WriteLine(CountRanges(ranges, "in", flows));
        }

        static long CountRanges(Dictionary<char, (long, long)> ranges, string workflowName, Dictionary<string, string[]> workflows)
        {
            if (workflowName == "R")
            {
                return 0;
            }

            if (workflowName == "A")
            {
                return ranges.Values.Aggregate(1, (long acc, (long, long) range) => acc * (range.Item2 - range.Item1 + 1));
            }

            var workflow = workflows[workflowName];
            var total = 0L;

            foreach (var rule in workflow)
            {
                var parts = rule.Contains(':') ? rule.Split(":") : new string[] { "-", rule };
                var condition = parts[0];
                var next = parts[1];

                if ("xmas".Contains(condition[0]))
                {
                    var cat = condition[0];
                    var op = condition[1];
                    var rightVal = long.Parse(condition[2..]);

                    var range = ranges[cat];

                    var trueForCondition = op == '<' ? (range.Item1, rightVal - 1) : (rightVal + 1, range.Item2);
                    var falseForCondition = op == '<' ? (rightVal, range.Item2) : (range.Item1, rightVal);

                    if (trueForCondition.Item1 <= trueForCondition.Item2)
                    {
                        var rangesCopy = new Dictionary<char, (long, long)>(ranges);
                        rangesCopy[cat] = trueForCondition;
                        total += CountRanges(rangesCopy, next, workflows);
                    }

                    if (falseForCondition.Item1 <= falseForCondition.Item2)
                    {
                        ranges[cat] = falseForCondition;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    total += CountRanges(ranges, next, workflows);
                }
            }

            return total;
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
