
namespace Day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                seeds: 79 14 55 13

                a-to-b map:
                50 98 2
                52 50 48

                b-to-c map:
                0 15 37
                37 52 2
            */

            var lines = File.ReadLines("input.txt");
            var enumerator = lines.GetEnumerator();

            enumerator.MoveNext();
            var seedsLine = enumerator.Current;
            var seeds = seedsLine.Split(' ').Skip(1).Select(long.Parse).ToList();
            var categoryMappings = new List<List<(long DestStart, long SourceStart, long RangeLength)>>();

            while (enumerator.MoveNext())
            {
                var line = enumerator.Current;
                if (line == "")
                {
                    // Separator between mapping, i.e. start of new mapping.
                    categoryMappings.Add(new List<(long DestStart, long SourceStart, long RangeLength)>());

                    // Skip "a-to-b map:" line.
                    enumerator.MoveNext();
                }
                else
                {
                    var sp = line.Split(' ');
                    var destStart = long.Parse(sp[0]);
                    var sourceStart = long.Parse(sp[1]);
                    var range = long.Parse(sp[2]);
                    categoryMappings.Last().Add((destStart, sourceStart, range));
                }
            }

            Part1(seeds, categoryMappings);
            Part2(seeds, categoryMappings);
        }

        private static void Part1(List<long> seeds, List<List<(long DestStart, long SourceStart, long RangeLength)>> categoryMappings)
        {
            long? closest = null;

            foreach (var seed in seeds)
            {
                var source = seed;
                foreach (var categoryRanges in categoryMappings)
                {
                    var range = categoryRanges.FirstOrDefault(x => x.SourceStart <= source && source < x.SourceStart + x.RangeLength);
                    if (range != default)
                    {
                        source = range.DestStart + (source - range.SourceStart);
                    }
                }

                if (closest == null || source < closest)
                {
                    closest = source;
                }
            }

            Console.WriteLine($"Part 1: {closest}");
        }

        private static void Part2(List<long> seedsInput, List<List<(long DestStart, long SourceStart, long RangeLength)>> categoryMappings)
        {
            var seedRanges = seedsInput.Chunk(2).Select(x => (First: x[0], Last: x[0] + x[1] - 1)).ToList();

            var reverseCategoryMappings = categoryMappings.ToList();
            reverseCategoryMappings.Reverse();

            long? closest = null;

            for (var loc = 0L; closest == null; loc++)
            {
                var dest = loc;
                foreach (var categoryRanges in reverseCategoryMappings)
                {
                    var range = categoryRanges.FirstOrDefault(x => x.DestStart <= dest && dest < x.DestStart + x.RangeLength);
                    if (range != default)
                    {
                        dest = range.SourceStart + (dest - range.DestStart);
                    }
                }

                if (seedRanges.Any(x => x.First <= dest && dest <= x.Last))
                {
                    closest = loc;
                }
            }

            Console.WriteLine($"Part 2: {closest}");
        }
    }
}
