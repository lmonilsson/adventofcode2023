namespace Day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var cardLines = File.ReadAllLines("input.txt");
            var cards = cardLines.Select(ParseCard).ToList();

            Part1(cards);
            Part2(cards);
        }

        private static void Part1(List<Card> cards)
        {
            var totalPoints = cards
                .Select(card => card.Matches == 0 ? 0 : (int)Math.Pow(2, card.Matches - 1))
                .Sum();

            Console.WriteLine($"Part 1: {totalPoints}");
        }

        private static void Part2(List<Card> cards)
        {
            var cardCounts = cards.Select(c => 1L).ToList();
            var totalCards = (long) cards.Count;

            int c = 0;
            while (c < cardCounts.Count)
            {
                var cc = cardCounts[c];
                if (cc > 0)
                {
                    var toAdd = cards[c].Matches;
                    for (var j = c + 1; j < cardCounts.Count && toAdd > 0; j++)
                    {
                        if (cardCounts[j] > 0)
                        {
                            cardCounts[j]++;
                            totalCards++;
                            toAdd--;
                        }
                    }
                    cardCounts[c]--;
                }
                else
                {
                    c++;
                }
            }

            Console.WriteLine($"Part 2: {totalCards}");
        }

        private static Card ParseCard(string cardLine)
        {
            // Card   1: 66 90 67 | 54 26  5 74 25 67 73 61
            var cardSplit = cardLine.Split(": ");

            // Card   1
            var cardNumberSplit = cardSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cardNumber = int.Parse(cardNumberSplit[1]);

            // 66 90 67 | 54 22 26  5 74 25 67 73 61
            var numbersSplit = cardSplit[1].Split(" | ");
            var winnerNumbers = numbersSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var haveNumbers = numbersSplit[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            var matches = winnerNumbers.Intersect(haveNumbers).Count();

            return new Card(cardNumber, matches);
        }

        record Card(int CardNumber, int Matches);
    }
}
