namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var image = File.ReadLines("input.txt")
                .Select(row => row.ToList())
                .ToList();
            
            Part1(image);
            Part2(image);
        }

        private static void Part1(List<List<char>> image)
        {
            // Copy to not disturb input.
            image = image.Select(row => row.ToList()).ToList();

            for (var r = 0; r < image.Count; r++)
            {
                if (image[r].All(x => x == '.'))
                {
                    image.Insert(r, Enumerable.Repeat('.', image[r].Count).ToList());
                    r++; // Skip inserted row
                }
            }

            for (var c = 0; c < image[0].Count; c++)
            {
                if (image.All(x => x[c] == '.'))
                {
                    image.ForEach(row => row.Insert(c, '.'));
                    c++; // Skip inserted column
                }
            }

            var galaxies = image
                .SelectMany((row, r) => row.Select((chr, c) => new { chr, r, c }))
                .Where(x => x.chr == '#')
                .Select(x => (Row: x.r, Col: x.c))
                .ToList();

            var distSum = 0;
            for (var g1 = 0; g1 < galaxies.Count - 1; g1++)
            {
                for (var g2 = g1 + 1; g2 < galaxies.Count; g2++)
                {
                    distSum +=
                        Math.Abs(galaxies[g1].Row - galaxies[g2].Row) +
                        Math.Abs(galaxies[g1].Col - galaxies[g2].Col);
                }
            }

            Console.WriteLine($"Part 1: {distSum}");
        }

        private static void Part2(List<List<char>> image)
        {
            var galaxies = image
               .SelectMany((row, r) => row.Select((chr, c) => new { chr, r, c }))
               .Where(x => x.chr == '#')
               .Select(x => (Row: x.r, Col: x.c))
               .ToList();

            var emptyRows = Enumerable.Range(0, image.Count).Where(r => image[r].All(chr => chr == '.')).ToHashSet();
            var emptyCols = Enumerable.Range(0, image[0].Count).Where(c => image.All(row => row[c] == '.')).ToHashSet();

            long distSum = 0;
            for (var g1 = 0; g1 < galaxies.Count - 1; g1++)
            {
                for (var g2 = g1 + 1; g2 < galaxies.Count; g2++)
                {
                    var minRow = Math.Min(galaxies[g1].Row, galaxies[g2].Row);
                    var maxRow = Math.Max(galaxies[g1].Row, galaxies[g2].Row);
                    var minCol = Math.Min(galaxies[g1].Col, galaxies[g2].Col);
                    var maxCol = Math.Max(galaxies[g1].Col, galaxies[g2].Col);

                    var numEmptyRowsBetween = Enumerable.Range(minRow, maxRow - minRow).Count(emptyRows.Contains);
                    var numEmptyColsBetween = Enumerable.Range(minCol, maxCol - minCol).Count(emptyCols.Contains);

                    var unexpandedRowDist = maxRow - minRow;
                    var expandedRowDist = (unexpandedRowDist - numEmptyRowsBetween) + numEmptyRowsBetween * 1_000_000L;

                    var unexpandedColDist = maxCol - minCol;
                    var expandedColDist = (unexpandedColDist - numEmptyColsBetween) + numEmptyColsBetween * 1_000_000L;

                    distSum += expandedRowDist + expandedColDist;
                }
            }

            Console.WriteLine($"Part 2: {distSum}");
        }
    }
}
