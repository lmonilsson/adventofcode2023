
namespace Day02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gameLines = File.ReadAllLines("input.txt");
            var games = gameLines.Select(ParseGame).ToList();
            Part1(games);
            Part2(games);
        }

        private static void Part1(IReadOnlyCollection<Game> games)
        {
            // Determine which games would have been possible if the bag had been loaded with only 12 red cubes, 13 green cubes, and 14 blue cubes.
            // What is the sum of the IDs of those games?

            var validGameIds = games
                .Where(g => g.Draws.All(d =>
                    d.GetValueOrDefault("red") <= 12 &&
                    d.GetValueOrDefault("green") <= 13 &&
                    d.GetValueOrDefault("blue") <= 14))
                .Select(g => g.GameId);

            Console.WriteLine($"Part 1: {validGameIds.Sum()}");
        }

        private static void Part2(IReadOnlyCollection<Game> games)
        {
            // The power of a set of cubes is equal to the numbers of red, green, and blue cubes multiplied together.
            // For each game, find the minimum set of cubes that must have been present.
            // What is the sum of the power of these sets?

            var gamePowerSum = 0;
            foreach (var g in games)
            {
                var maxReds = g.Draws.Max(d => d.GetValueOrDefault("red"));
                var maxGreens = g.Draws.Max(d => d.GetValueOrDefault("green"));
                var maxBlues = g.Draws.Max(d => d.GetValueOrDefault("blue"));
                var gamePower = maxReds * maxGreens * maxBlues;
                gamePowerSum += gamePower;
            }

            Console.WriteLine($"Part 2: {gamePowerSum}");
        }

        private static Game ParseGame(string game)
        {
            // "Game 2: 1 green, 7 red; 1 green, 9 red, 3 blue; 4 blue, 5 red"
            var gameSplit = game.Split(": ");
            var gameId = int.Parse(gameSplit[0].Split(' ')[1]);

            var drawStrings = gameSplit[1].Split("; ");
            var draws = drawStrings.Select(ParseDraw).ToList();

            return new Game(gameId, draws);
        }

        private static IReadOnlyDictionary<string, int> ParseDraw(string draw)
        {
            // "1 green, 9 red, 3 blue"
            var cubes = draw.Split(", ");
            var colorCounts = new Dictionary<string, int>();
            foreach (var cube in cubes)
            {
                var cubeSplit = cube.Split(" ");
                colorCounts[cubeSplit[1]] = int.Parse(cubeSplit[0]);
            }

            return colorCounts;
        }

        record Game(int GameId, IReadOnlyCollection<IReadOnlyDictionary<string, int>> Draws);
    }
}
