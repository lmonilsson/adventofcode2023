
namespace Day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                0 3 6 9 12 15
                1 3 6 10 15 21
                10 13 16 21 30 45
             */

            var histories = File.ReadAllLines("input.txt")
                .Select(x => x.Split(' ').Select(int.Parse).ToList())
                .ToList();

            var extrapolated = histories.Select(Extrapolate).ToList();
            Console.WriteLine($"Part 1: {extrapolated.Sum()}");

            var historiesReversed = histories.Select(x => x.Reverse<int>().ToList()).ToList();
            var extrapolatedBackwards = historiesReversed.Select(Extrapolate).ToList();
            Console.WriteLine($"Part 2: {extrapolatedBackwards.Sum()}");
        }

        private static int Extrapolate(List<int> history)
        {
            var ends = new List<int>();
            var next = history;
            while(next.Any(x => x != 0))
            {
                ends.Add(next.Last());
                next = next.Zip(next.Skip(1), (a, b) => b - a).ToList();
            }

            ends.Reverse();
            var extrapolated = ends.Aggregate(0, (a, b) => a + b);
            return extrapolated;
        }
    }
}
