namespace Day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Time:        60     94     78     82
            // Distance:   475   2138   1015   1650

            var lines = File.ReadAllLines("input.txt");

            Part1(lines);
            Part2(lines);
        }

        private static void Part1(string[] lines)
        {
            var raceTimes = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();
            var recordDistances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(int.Parse).ToList();

            var numberOfWinningStrategiesPerRace = new List<int>();
            for (var i = 0; i < raceTimes.Count; i++)
            {
                var time = raceTimes[i];
                var record = recordDistances[i];

                var winningStrategies = 0;
                for (var speed = 1; speed < time; speed++)
                {
                    var moveTime = time - speed;
                    var distance = moveTime * speed;
                    if (distance > record)
                    {
                        winningStrategies++;
                    }
                }

                numberOfWinningStrategiesPerRace.Add(winningStrategies);
            }

            var winningStrategiesMultiplied = numberOfWinningStrategiesPerRace.Aggregate(1, (a, b) => a * b);
            Console.WriteLine($"Part 1: {winningStrategiesMultiplied}");
        }

        private static void Part2(string[] lines)
        {
            var raceTime = long.Parse(string.Join("", lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));
            var recordDistance = long.Parse(string.Join("", lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));

            var winningStrategies = 0L;
            for (var speed = 1L; speed < raceTime; speed++)
            {
                var moveTime = raceTime - speed;
                var distance = moveTime * speed;
                if (distance > recordDistance)
                {
                    winningStrategies++;
                }
            }

            Console.WriteLine($"Part 1: {winningStrategies}");
        }
    }
}
