namespace AdventOfCode2023
{
    internal class D15 : Day
    {
        internal override void SolvePart1()
        {
            var commands = FileHelper.ReadLines(FileName).First().Split(',');

            long res = 0;

            foreach (var command in commands)
            {
                res += Hash(command);
            }

            Console.WriteLine(res);
        }

        internal override void SolvePart2()
        {
            var commands = FileHelper.ReadLines(FileName).First().Split(',');

            var boxes = Execute(commands);

            long res = 0;

            foreach (var (box, i) in boxes.WithIndex())
            {
                foreach (var (lens, j) in box.Reverse().WithIndex())
                {
                    res += (i + 1) * (j + 1) * lens.FL;
                }
            }

            Console.WriteLine(res);
        }

        private static int Hash(string command)
        {
            int p = 0;

            foreach (var c in command)
            {
                p += c;
                p *= 17;
                p %= 256;
            }

            return p;
        }

        private static Stack<(string L, int FL)>[] Execute(string[] commands)
        {
            var boxes = new Stack<(string L, int FL)>[256];
            for (int i = 0; i < 256; i++) boxes[i] = new Stack<(string L, int FL)>();

            foreach (var c in commands)
            {
                var label = c.EndsWith('-') ? c[..^1] : c[..^2];
                var i = Hash(label);

                if (c.EndsWith('-'))
                {
                    boxes[i] = new Stack<(string L, int FL)>(boxes[i].Where(l => l.L != label).Reverse());
                }
                else
                {
                    var fl = int.Parse(c[^1..]);
                    var slot = boxes[i].FirstOrDefault(l => l.L == label);
                    if (slot.L == null)
                    {
                        boxes[i].Push((label, fl));
                    }
                    else
                    {
                        boxes[i] = new Stack<(string L, int FL)>(boxes[i].Select(l => l.L == label ? (label, fl) : l).Reverse());
                    }
                }
            }

            return boxes;
        }
    }
}
