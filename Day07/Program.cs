

namespace Day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Part1(lines);
            Part2(lines);
        }

        private static void Part1(string[] lines)
        {
            var hands = lines.Select(ParseHandPart1).ToList();
            var groupedByType = hands.GroupBy(GetHandTypeRankPart1).ToList();
            var sortedWithinGroups = groupedByType
                .OrderBy(g => g.Key)
                .Select(g => g.OrderBy(h => h.Cards, new HandComparer()).ToList()).ToList();
            var allHandsSorted = sortedWithinGroups.SelectMany(g => g).ToList();
            var handWinnings = allHandsSorted.Select((x, i) => x.Bet * (i + 1)).ToList();
            var totalWinnings = handWinnings.Sum();
            Console.WriteLine($"Part 1: {totalWinnings}");
        }

        private static int GetHandTypeRankPart1(Hand hand)
        {
            var cardCounts = Enumerable.Repeat(0, 15).ToList();
            hand.Cards.ForEach(x => cardCounts[x]++);

            if (cardCounts.Any(x => x == 5))
            {
                return 7;
            }
            
            if (cardCounts.Any(x => x == 4))
            {
                return 6;
            }
            
            if (cardCounts.Any(x => x == 3) && cardCounts.Any(x => x == 2))
            {
                return 5;
            }
            
            if (cardCounts.Any(x => x == 3))
            {
                return 4;
            }
            
            var pair1Index = cardCounts.FindIndex(x => x == 2);
            var pair2Index = pair1Index >= 0 ? cardCounts.FindIndex(pair1Index + 1, x => x == 2) : -1;

            if (pair1Index >= 0 && pair2Index >= 0)
            {
                return 3;
            }

            if (pair1Index >= 0)
            {
                return 2;
            }

            // High card
            return 1;
        }

        private static Hand ParseHandPart1(string line)
        {
            // Q379J 837
            var sp = line.Split(' ');
            var cards = sp[0].Select(GetCardValuePart1).ToList();
            var bet = int.Parse(sp[1]);
            return new Hand(cards, bet);
        }

        private static int GetCardValuePart1(char c)
        {
            if (char.IsDigit(c))
            {
                return c - 50;
            }

            switch (c)
            {
                case 'T': return 8;
                case 'J': return 9;
                case 'Q': return 10;
                case 'K': return 11;
                case 'A': return 12;
                default: throw new ArgumentException($"Invalid card {c}", nameof(c));
            }
        }
        private static void Part2(string[] lines)
        {
            var hands = lines.Select(ParseHandPart2).ToList();
            var groupedByType = hands.GroupBy(GetHandTypeRankPart2).ToList();
            var sortedWithinGroups = groupedByType
                .OrderBy(g => g.Key)
                .Select(g => g.OrderBy(h => h.Cards, new HandComparer()).ToList()).ToList();
            var allHandsSorted = sortedWithinGroups.SelectMany(g => g).ToList();
            var handWinnings = allHandsSorted.Select((x, i) => x.Bet * (i + 1)).ToList();
            var totalWinnings = handWinnings.Sum();
            Console.WriteLine($"Part 2: {totalWinnings}");
        }

        private static int GetHandTypeRankPart2(Hand hand)
        {
            var cardCounts = Enumerable.Repeat(0, 14).ToList();
            foreach (var card in hand.Cards.Where(x => x != 0))
            {
                cardCounts[card - 1]++;
            }
            var wc = hand.Cards.Count(x => x == 0);

            if (cardCounts.Any(x => x + wc == 5))
            {
                return 7;
            }
            if (cardCounts.Any(x => x + wc == 4))
            {
                return 6;
            }

            var threeIndex = cardCounts.FindIndex(x => x + wc == 3);
            if (threeIndex >= 0)
            {
                var twoIndex = cardCounts.FindIndex(x => x == 2);
                if (twoIndex == threeIndex)
                {
                    twoIndex = cardCounts.FindIndex(threeIndex + 1, x => x == 2);
                }

                if (twoIndex >= 0)
                {
                    // Full house
                    return 5;
                }

                // Three of a kind
                return 4;
            }

            var pair1Index = cardCounts.FindIndex(x => x + wc == 2);
            if (pair1Index >= 0)
            {
                var pair2Index = cardCounts.FindIndex(x => x == 2);
                if (pair2Index == pair1Index)
                {
                    pair2Index = cardCounts.FindIndex(pair1Index + 1, x => x == 2);
                }

                if (pair2Index >= 0)
                {
                    // Two pairs
                    return 3;
                }

                // One pair
                return 2;
            }

            // High card
            return 1;
        }

        private static Hand ParseHandPart2(string line)
        {
            // Q379J 837
            var sp = line.Split(' ');
            var cards = sp[0].Select(GetCardValuePart2).ToList();
            var bet = int.Parse(sp[1]);
            return new Hand(cards, bet);
        }

        private static int GetCardValuePart2(char c)
        {
            if (char.IsDigit(c))
            {
                return c - 49;
            }

            switch (c)
            {
                case 'T': return 9;
                case 'J': return 0;
                case 'Q': return 10;
                case 'K': return 11;
                case 'A': return 12;
                default: throw new ArgumentException($"Invalid card {c}", nameof(c));
            }
        }

        record Hand(List<int> Cards, int Bet);
    }

    internal class HandComparer : IComparer<List<int>>
    {
        public int Compare(List<int>? x, List<int>? y)
        {
            if (x == null || y == null) throw new ArgumentException("null not allowed");

            for (var i = 0; i < x.Count; i++)
            {
                if (x[i] < y[i]) return -1;
                if (x[i] > y[i]) return 1;
            }

            return 0;
        }
    }
}
