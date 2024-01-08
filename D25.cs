namespace AdventOfCode2023
{
    internal class D25 : Day
    {
        internal override void SolvePart1()
        {
            var graph = new Dictionary<string, HashSet<string>>();
            var lines = FileHelper.ReadLines(FileName);
            foreach (var line in lines)
            {
                var parts = line.Split(": ");
                string u = parts[0];
                var vs = parts[1].Split(" ");

                if (!graph.ContainsKey(u))
                {
                    graph[u] = new HashSet<string>();
                }

                foreach (var v in vs)
                {
                    graph[u].Add(v);
                    if (!graph.ContainsKey(v))
                    {
                        graph[v] = new HashSet<string>();
                    }
                    graph[v].Add(u);
                }
            }

            var subset = new HashSet<string>(graph.Keys);

            int Count(string v) => graph[v].Count(x => !subset.Contains(x));

            while (subset.Select(count => Count(count)).Sum() != 3)
            {
                subset.Remove(subset.MaxBy(Count));
            }

            Console.WriteLine(subset.Count * (graph.Keys.Count - subset.Count));
        }

        internal override void SolvePart2()
        {

        }
    }
}
