namespace AdventOfCode2023
{
    internal class D07 : Day
    {
        internal override void SolvePart1()
        {
            var hands = FileHelper.ReadLines(FileName).Select(ToHand<NormalHand>).Order().ToArray();
            Console.WriteLine(CalculateWinnings(hands));
        }

        internal override void SolvePart2()
        {
            var hands = FileHelper.ReadLines(FileName).Select(ToHand<JokerHand>).Order().ToArray();
            Console.WriteLine(CalculateWinnings(hands));
        }

        private static Hand ToHand<T>(string l) where T : Hand, new()
        {
            var data = l.Split(' ');
            return new T { Cards = data[0], Bid = long.Parse(data[1]) };
        }

        private static long CalculateWinnings(Hand[] hands)
        {
            return hands.Select((h, i) => (i + 1) * h.Bid).Sum();
        }
    }

    internal abstract class Hand : IComparable
    {
        internal string Cards { get; set; }

        internal long Bid { get; set; }

        protected string CardOrder { get; set; }

        public int CompareTo(object obj)
        {
            var other = (Hand)obj;

            var r1 = Rank();
            var r2 = other.Rank();

            if (r1 != r2) return r1 < r2 ? -1 : 1;
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    int p1 = CardOrder.IndexOf(Cards[i]);
                    int p2 = CardOrder.IndexOf(other.Cards[i]);
                    if (p1 < p2) return -1;
                    if (p1 > p2) return 1;
                }
            }
            return 0;
        }

        protected int BaseRank()
        {
            return Cards.Distinct().Count() switch
            {
                5 => 1,
                4 => 2,
                3 => Cards.GroupBy(c => c).Select(g => g.Count()).Max() == 2 ? 3 : 4,
                2 => Cards.GroupBy(c => c).Select(g => g.Count()).Max() == 3 ? 5 : 6,
                1 => 7,
                _ => 0
            };
        }

        protected abstract int Rank();
    }

    internal class NormalHand : Hand
    {
        public NormalHand()
        {
            CardOrder = "23456789TJQKA";
        }

        protected override int Rank()
        {
            return BaseRank();
        }
    }

    internal class JokerHand : Hand
    {
        public JokerHand()
        {
            CardOrder = "J23456789TQKA";
        }

        protected override int Rank()
        {
            return CardOrder[1..]
                   .Select(c => new JokerHand { Cards = Cards.Replace('J', c), Bid = Bid }.BaseRank())
                   .Max();
        }
    }
}
