namespace AdventOfCode2023
{
    internal class D17 : Day
    {
        internal override void SolvePart1()
        {
            var map = FileHelper.ReadLinesAsIntMap(FileName);
            Console.WriteLine(GetCostCrucible(map, 0, 3));
        }

        internal override void SolvePart2()
        {
            var map = FileHelper.ReadLinesAsIntMap(FileName);
            Console.WriteLine(GetCostCrucible(map, 4, 10));
        }

        private static int GetCostCrucible(int[][] map, int minf, int maxf)
        {
            int height = map.Length;
            int width = map[0].Length;

            var visited = new Dictionary<(int, int, int, int), int>();

            var priorityQueue = new PriorityQueue<(int x, int y, int d, int r, int c), int>();
            priorityQueue.Enqueue((0, 0, 0, maxf, map[0][0]), 0);

            while (priorityQueue.Count > 0)
            {
                var node = priorityQueue.Dequeue();
                if (visited.GetValueOrDefault((node.x, node.y, node.d, node.r)) != 0) continue;
                visited[(node.x, node.y, node.d, node.r)] = node.c;

                foreach (var n in GetNeighborsCrucible(node, minf, maxf))
                {
                    if (n.x >= 0 && n.y >= 0 && n.x < height && n.y < width && visited.GetValueOrDefault((n.x, n.y, n.d, n.r)) == 0)
                    {
                        var nc = node.c + map[n.x][n.y];
                        priorityQueue.Enqueue((n.x, n.y, n.d, n.r, nc), nc);
                    }
                }
            }

            int result = int.MaxValue;
            for (int d = 0; d < 4; d++)
            {
                for (int r = 0; r < maxf - minf; r++)
                {
                    var t = visited.GetValueOrDefault((height - 1, width - 1, d, r));
                    result = Math.Min(result, t == 0 ? int.MaxValue : t);
                }
            }
            return result - map[0][0];
        }

        private static List<(int x, int y, int d, int r)> GetNeighborsCrucible((int x, int y, int d, int r, int c) node, int minf, int maxf)
        {
            var neighbours = new List<(int x, int y, int d, int r)>();
            var ds = new (int x, int y)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
            if (node.r > 0)
            {
                neighbours.Add((node.x + ds[node.d].x, node.y + ds[node.d].y, node.d, node.r - 1));
            }
            if (node.r <= maxf - minf)
            {
                var nd = (node.d + 1) % 4;
                neighbours.Add((node.x + ds[nd].x, node.y + ds[nd].y, nd, maxf - 1));
                nd = (node.d + 3) % 4;
                neighbours.Add((node.x + ds[nd].x, node.y + ds[nd].y, nd, maxf - 1));
            }
            return neighbours;
        }
    }
}
