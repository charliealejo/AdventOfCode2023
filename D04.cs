using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    internal partial class D04 : Day
    {
        internal override void SolvePart1()
        {
            var lines = FileHelper.ReadLines(FileName);

            long r = 0;
            foreach (var item in lines)
            {
                var ns = Numbers().Matches(item).Select(x => int.Parse(x.Value)).ToArray();
                var wn = ns[1..11];
                var hn = ns[11..];
                r += (long)Math.Pow(2, hn.Where(h => wn.Contains(h)).Count() - 1);
            }

            Console.WriteLine(r);
        }

        internal override void SolvePart2()
        {
            var lines = FileHelper.ReadLines(FileName);

            var cards = new List<int[]>();
            foreach (var item in lines)
            {
                var card = new List<int> { 1 };
                card.AddRange(Numbers().Matches(item).Select(x => int.Parse(x.Value)));
                cards.Add(card.ToArray());
            }

            foreach (var card in cards)
            {
                var wn = card[2..12];
                var hn = card[12..];
                var m = hn.Where(h => wn.Contains(h)).Count();
                for (int i = 0; i < m; i++)
                {
                    cards[card[1] + i][0] += card[0];
                }
            }

            Console.WriteLine(cards.Select(c => c[0]).Sum());
        }

        [GeneratedRegex("[0-9]+")]
        private static partial Regex Numbers();
    }
}
