var lines = File.ReadAllLines("input.txt");
var part1 = lines
    .Select(line => line.First(char.IsDigit).ToString() + line.Last(char.IsDigit))
    .Select(x => Convert.ToInt32(x))
    .Sum();

Console.WriteLine($"Part 1: {part1}");

var spelledDigits = new Dictionary<string, int>
{
    { "1", 1 },{ "2", 2 },{ "3", 3 },{ "4", 4 },{ "5", 5 },
    { "6", 6 },{ "7", 7 },{ "8", 8 },{ "9", 9 },
    { "one", 1 }, { "two", 2 }, {"three", 3 }, {"four", 4 }, {"five", 5 },
    { "six", 6 }, { "seven",7 }, { "eight", 8 }, {"nine",9 }
};

var part2 = lines
    .Select(line =>
    {
        var firstKey = spelledDigits.Keys.Select(x => (x, line.IndexOf(x))).Where(xi => xi.Item2 >= 0).MinBy(xi => xi.Item2).Item1;
        var lastKey = spelledDigits.Keys.Select(x => (x, line.LastIndexOf(x))).Where(xi => xi.Item2 >= 0).MaxBy(xi => xi.Item2).Item1;
        return Convert.ToInt32(spelledDigits[firstKey].ToString() + spelledDigits[lastKey]);
    })
    .Sum();

Console.WriteLine($"Part 2: {part2}");