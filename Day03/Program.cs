
namespace Day03
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
            var digits = Enumerable.Range('0', 10).Select(i => (char)i).ToArray();
            var sumAdjacentToSymbol = 0;

            for (var r = 0; r < lines.Length; r++)
            {
                var line = lines[r];
                var first = line.IndexOfAny(digits);
                while (first >= 0)
                {
                    var last = first;
                    while (last < line.Length - 1 && char.IsDigit(line[last + 1]))
                    {
                        last++;
                    }

                    var adjacentToSymbol = false;
                    for (int c = first; c <= last; c++)
                    {
                        if (r > 0 && c > 0 && IsSymbol(lines[r - 1][c - 1]))
                        {
                            adjacentToSymbol = true;
                        }
                        if (r > 0 && IsSymbol(lines[r - 1][c]))
                        {
                            adjacentToSymbol = true;
                        }
                        if (r > 0 && c < line.Length - 1 && IsSymbol(lines[r - 1][c + 1]))
                        {
                            adjacentToSymbol = true;
                        }
                        if (c > 0 && IsSymbol(lines[r][c - 1]))
                        {
                            adjacentToSymbol = true;
                        }
                        if (c < line.Length - 1 && IsSymbol(lines[r][c + 1]))
                        {
                            adjacentToSymbol = true;
                        }
                        if (r < lines.Length - 1 && c > 0 && IsSymbol(lines[r + 1][c - 1]))
                        {
                            adjacentToSymbol = true;
                        }
                        if (r < lines.Length - 1 && IsSymbol(lines[r + 1][c]))
                        {
                            adjacentToSymbol = true;
                        }
                        if (r < lines.Length - 1 && c < line.Length - 1 && IsSymbol(lines[r + 1][c + 1]))
                        {
                            adjacentToSymbol = true;
                        }
                    }

                    if (adjacentToSymbol)
                    {
                        var partNumber = int.Parse(line.Substring(first, last - first + 1));
                        sumAdjacentToSymbol += partNumber;
                    }

                    first = line.IndexOfAny(digits, last + 1);
                }
            }

            Console.WriteLine($"Part 1: {sumAdjacentToSymbol}");
        }

        static void Part2(string[] lines)
        {
            var symbols = lines.SelectMany(line => line.Where(IsSymbol)).Distinct().ToArray();
            var gearRatioSum = 0L;

            for (var r = 0; r < lines.Length; r++)
            {
                var line = lines[r];
                var c = line.IndexOfAny(symbols);
                while (c >= 0)
                {
                    var adjacentParts = new List<int>();

                    if (r > 0)
                    {
                        if (IsDigit(lines[r - 1][c]))
                        {
                            adjacentParts.Add(ParseGear(lines[r - 1], c));
                        }
                        else
                        {
                            if (c > 0 && IsDigit(lines[r - 1][c - 1]))
                            {
                                adjacentParts.Add(ParseGear(lines[r - 1], c - 1));
                            }
                            if (c < line.Length - 1 && IsDigit(lines[r - 1][c + 1]))
                            {
                                adjacentParts.Add(ParseGear(lines[r - 1], c + 1));
                            }
                        }
                    }

                    if (c > 0 && IsDigit(lines[r][c - 1]))
                    {
                        adjacentParts.Add(ParseGear(lines[r], c - 1));
                    }
                    if (c < line.Length - 1 && IsDigit(lines[r][c + 1]))
                    {
                        adjacentParts.Add(ParseGear(lines[r], c + 1));
                    }

                    if (r < lines.Length - 1)
                    {
                        if (IsDigit(lines[r + 1][c]))
                        {
                            adjacentParts.Add(ParseGear(lines[r + 1], c));
                        }
                        else
                        {
                            if (c > 0 && IsDigit(lines[r + 1][c - 1]))
                            {
                                adjacentParts.Add(ParseGear(lines[r + 1], c - 1));
                            }
                            if (c < line.Length - 1 && IsDigit(lines[r + 1][c + 1]))
                            {
                                adjacentParts.Add(ParseGear(lines[r + 1], c + 1));
                            }
                        }
                    }

                    if (adjacentParts.Count == 2)
                    {
                        var gearRatio = adjacentParts[0] * adjacentParts[1];
                        gearRatioSum += gearRatio;
                    }

                    c = line.IndexOfAny(symbols, c + 1);
                }
            }

            Console.WriteLine($"Part 2: {gearRatioSum}");
        }

        private static int ParseGear(string line, int c)
        {
            var begin = c;
            while (begin > 0 && IsDigit(line[begin - 1])) begin--;

            var end = c;
            while (end < line.Length - 1 && IsDigit(line[end + 1])) end++;

            return int.Parse(line.Substring(begin, end - begin + 1));
        }

        public static bool IsDigit(char c)
        {
            return char.IsDigit(c);
        }

        public static bool IsSymbol(char c)
        {
            return !IsDigit(c) && c != '.';
        }
    }
}
