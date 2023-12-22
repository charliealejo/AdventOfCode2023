namespace AdventOfCode2023
{
    internal class D20 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);
            var modules = GetModuleTree(lines);

            var sp = new SignalProcessor(modules["broadcaster"]);
            for (int i = 0; i < 1000; i++)
                sp.Process();

            Console.WriteLine(modules.Values.Select(v => v.Highs).Sum()
                            * modules.Values.Select(v => v.Lows).Sum());
        }

        private long i = 0;
        private Conjunction lastModule = null;
        private readonly Dictionary<string, long> loops = new();

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName);
            var modules = GetModuleTree(lines);

            lastModule = modules.Values.Where(m => m.Destinations.Select(d => d.Name).Contains("rx")).First() as Conjunction;
            lastModule.HighRaised += SetLoopValue;

            var sp = new SignalProcessor(modules["broadcaster"]);
            while (true)
            {
                i++;
                sp.Process();
                if (loops.Keys.Count == lastModule.InputStates.Count) break;
            }

            Console.WriteLine(loops.Values.Product());
        }

        private void SetLoopValue(string name)
        {
            loops[name] = i;
        }

        private static Dictionary<string, Module> GetModuleTree(IEnumerable<string> lines)
        {
            var d = new Dictionary<string, Module>();

            foreach (var line in lines)
            {
                var parts = line.Split(" -> ");
                var name = parts[0];
                if (name[0] == '%')
                {
                    d.Add(name[1..], new FlipFlop { Name = name[1..] });
                }
                else if (name[0] == '&')
                {
                    d.Add(name[1..], new Conjunction { Name = name[1..] });
                }
                else d.Add(name, new Broadcaster { Name = name });
            }
            foreach (var line in lines)
            {
                var parts = line.Split(" -> ");
                var name = parts[0];
                if (name[0] == '%' || name[0] == '&') name = name[1..];
                var destinations = parts[1].Split(", ");
                foreach (var dest in destinations)
                {
                    if (d.ContainsKey(dest))
                    {
                        d[name].Destinations.Add(d[dest]);

                        if (d[dest] is Conjunction c)
                        {
                            c.InputStates.Add(name, false);
                        }
                    }
                    else
                    {
                        d[dest] = new Output { Name = dest };
                        d[name].Destinations.Add(d[dest]);
                    }
                }
            }

            return d;
        }
    }

    internal abstract class Module
    {
        internal string Name { get; set; }

        internal long Lows { get; set; } = 0;

        internal long Highs { get; set; } = 0;

        internal List<Module> Destinations { get; set; } = new List<Module>();

        internal virtual void Signal(bool high, string name, SignalProcessor sp)
        {
            if (high) Highs++; else Lows++;

            if (high)
            {
                HighRaised?.Invoke(name);
            }
        }

        internal delegate void HighRaisedHandler(string name);

        internal event HighRaisedHandler HighRaised;
    }

    internal class Broadcaster : Module
    {
        internal override void Signal(bool high, string name, SignalProcessor sp)
        {
            base.Signal(high, name, sp);
            foreach (var dest in Destinations) sp.Enqueue(dest, high, Name);
        }
    }

    internal class Output : Module
    {
        internal override void Signal(bool high, string name, SignalProcessor sp)
        {
            base.Signal(high, name, sp);
        }
    }

    internal class FlipFlop : Module
    {
        private bool state = false;

        internal override void Signal(bool high, string name, SignalProcessor sp)
        {
            base.Signal(high, name, sp);
            if (!high)
            {
                state = !state;
                foreach (var dest in Destinations) sp.Enqueue(dest, state, Name);
            }
        }
    }

    internal class Conjunction : Module
    {
        internal Dictionary<string, bool> InputStates = new();

        internal override void Signal(bool high, string name, SignalProcessor sp)
        {
            base.Signal(high, name, sp);
            InputStates[name] = high;
            foreach (var dest in Destinations) sp.Enqueue(dest, !InputStates.Values.All(v => v), Name);
        }
    }

    internal class SignalProcessor
    {
        private readonly Module root;

        private Queue<(Module m, bool s, string prevName)> queue;

        internal SignalProcessor(Module broadcaster)
        {
            root = broadcaster;
            queue = new Queue<(Module, bool, string)>();
        }

        internal void Enqueue(Module m, bool s, string p)
        {
            queue.Enqueue((m, s, p));
        }

        internal void Process()
        {
            queue.Enqueue((root, false, "button"));
            while (queue.Count > 0)
            {
                var (m, s, p) = queue.Dequeue();
                m.Signal(s, p, this);
            }
        }
    }
}
