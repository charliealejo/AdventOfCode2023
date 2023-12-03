namespace AdventOfCode2023
{
    internal static class Program
    {
        static void Main()
        {
            for (int i = 1; i <= 25; i++)
            {
                var t = Type.GetType($"AdventOfCode2023.D{i:D2}");
                if (t != null)
                {
                    if (Activator.CreateInstance(t) is Day day)
                    {
                        Console.WriteLine($"Solutions for day {i}:");
                        day.SolvePart1();
                        day.SolvePart2();
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
