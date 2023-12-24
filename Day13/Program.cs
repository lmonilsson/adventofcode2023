
namespace Day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var patterns = File.ReadAllText("input.txt").Trim()
                .Split("\n\n")
                .Select(pattern => pattern.Split('\n').Select(line => line.ToArray()).ToArray())
                .ToList();

            Part1(patterns);
            Part2(patterns);
        }

        private static void Part1(List<char[][]> patterns)
        {
            var summarized = patterns.Select(p =>
            {
                var (v, h) = FindReflectionPoint(p, null);
                if (v != null)
                    return v.Value;
                else if (h != null)
                    return h.Value * 100;
                else
                    throw new Exception("No reflection point found!");
            }).Sum();

            Console.WriteLine($"Part 1: {summarized}");
        }

        private static (int? Vertical, int? Horizontal) FindReflectionPoint(char[][] pattern, (int? V, int? H)? ignorePoint)
        {
            for (int v = 1; v < pattern[0].Length; v++)
            {
                if (v == ignorePoint?.V)
                    continue;

                var mirror = true;
                var steps = Math.Min(v, pattern[0].Length - v);
                for (int i = 0; i < steps; i++)
                {
                    var c1 = pattern.Select(x => x[v - i - 1]);
                    var c2 = pattern.Select(x => x[v + i]);
                    if (!c1.SequenceEqual(c2))
                    {
                        mirror = false;
                        break;
                    }
                }

                if (mirror)
                {
                    return (v, null);
                }
            }

            for (int h = 1; h < pattern.Length; h++)
            {
                if (h == ignorePoint?.H)
                    continue;

                var mirror = true;
                var steps = Math.Min(h, pattern.Length - h);
                for (int i = 0; i < steps; i++)
                {
                    var r1 = pattern[h - i - 1];
                    var r2 = pattern[h + i];
                    if (!r1.SequenceEqual(r2))
                    {
                        mirror = false;
                        break;
                    }
                }

                if (mirror)
                {
                    return (null, h);
                }
            }

            return (null, null);
        }

        private static void Part2(List<char[][]> patterns)
        {
            var summarized = patterns.Select(p =>
            {
                var (v, h) = FindNewReflectionPointWithOneSmudge(p);
                if (v != null)
                    return v.Value;
                else if (h != null)
                    return h.Value * 100;
                else
                    throw new Exception("No reflection point found!");
            }).Sum();

            Console.WriteLine($"Part 2: {summarized}");
        }

        private static (int? Vertical, int? Horizontal) FindNewReflectionPointWithOneSmudge(char[][] pattern)
        {
            var originalPoint = FindReflectionPoint(pattern, null);

            for (int s = 0; s < pattern.Length * pattern[0].Length; s++)
            {
                var sr = s / pattern[0].Length;
                var sc = s % pattern[0].Length;
                pattern[sr][sc] = ToggleSmudge(pattern[sr][sc]);
                var (v, h) = FindReflectionPoint(pattern, originalPoint);
                pattern[sr][sc] = ToggleSmudge(pattern[sr][sc]);

                if (v != null || h != null)
                {
                    return (v, h);
                }
            }

            throw new Exception("No reflection point found!");
        }

        private static char ToggleSmudge(char c)
        {
            return c == '.' ? '#' : '.';
        }
    }
}
