namespace AdventOfCode2023
{
    internal abstract class Day
    {
        internal string FileName => $"{GetType().Name}.txt";

        internal abstract void SolvePart1();

        internal abstract void SolvePart2();
    }
}
