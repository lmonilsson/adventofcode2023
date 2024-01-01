using System.Diagnostics;

namespace Day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rows = File.ReadLines("input.txt").Select(line => line.ToList()).ToList();
            Part1(rows);
            Part2(rows);
        }

        private static void Part1(List<List<char>> input)
        {
            var platform = input.Select(r => r.ToList()).ToList();
            TiltNorth(platform);
            Console.WriteLine($"Part 1: {CountLoad(platform)}");
        }

        private static void Part2(List<List<char>> input)
        {
            var platform = input.Select(r => r.ToList()).ToList();
            var visitedPlatformIndices = new Dictionary<string, int>();
            int cycle = -1;
            int loopStart = -1, loopEnd = -1;
            while (true)
            {
                SpinCycle(platform);
                cycle++;
                if (visitedPlatformIndices.TryGetValue(Stringify(platform), out var cl))
                {
                    loopStart = cl;
                    loopEnd = cycle;
                    break;
                }
                visitedPlatformIndices[Stringify(platform)] = cycle;
            }

            var loopLength = loopEnd - loopStart;
            var platformStringsInLoop = visitedPlatformIndices.OrderBy(kvp => kvp.Value)
                .Skip(loopStart)
                .Take(loopLength)
                .Select(kvp => kvp.Key)
                .ToList();
            var lastCycleIndex = 1_000_000_000 - 1;
            var finalPlatformString = platformStringsInLoop[(lastCycleIndex - loopStart) % loopLength];
            var finalPlatform = Unstringify(finalPlatformString, platform[0].Count);

            Console.WriteLine($"Part 2: {CountLoad(finalPlatform)}");
        }

        private static List<List<char>> Unstringify(string str, int width)
        {
            return str.Chunk(width).Select(x => x.ToList()).ToList();
        }

        private static string Stringify(List<List<char>> moved)
        {
            return string.Join("", moved.SelectMany(r => string.Join("", r)));
        }

        private static void SpinCycle(List<List<char>> rows)
        {
            TiltNorth(rows);
            TiltWest(rows);
            TiltSouth(rows);
            TiltEast(rows);
        }

        private static void TiltNorth(List<List<char>> rows)
        {
            for (int r = 1; r < rows.Count; r++)
            {
                for (int c = 0; c < rows[r].Count; c++)
                {
                    if (rows[r][c] == 'O')
                    {
                        int r2 = r - 1;
                        for (; r2 >= 0; r2--)
                        {
                            if (rows[r2][c] != '.')
                            {
                                break;
                            }
                        }

                        rows[r][c] = '.';
                        rows[r2 + 1][c] = 'O';
                    }
                }
            }
        }

        private static void TiltSouth(List<List<char>> rows)
        {
            for (int r = rows.Count - 2; r >= 0; r--)
            {
                for (int c = 0; c < rows[r].Count; c++)
                {
                    if (rows[r][c] == 'O')
                    {
                        int r2 = r + 1;
                        for (; r2 < rows.Count; r2++)
                        {
                            if (rows[r2][c] != '.')
                            {
                                break;
                            }
                        }

                        rows[r][c] = '.';
                        rows[r2 - 1][c] = 'O';
                    }
                }
            }
        }

        private static void TiltWest(List<List<char>> rows)
        {
            for (int c = 1; c < rows[0].Count; c++)
            {
                for (int r = 0; r < rows.Count; r++)
                {
                    if (rows[r][c] == 'O')
                    {
                        int c2 = c - 1;
                        for (; c2 >= 0; c2--)
                        {
                            if (rows[r][c2] != '.')
                            {
                                break;
                            }
                        }

                        rows[r][c] = '.';
                        rows[r][c2 + 1] = 'O';
                    }
                }
            }
        }

        private static void TiltEast(List<List<char>> rows)
        {
            for (int c = rows[0].Count - 2; c >= 0; c--)
            {
                for (int r = 0; r < rows.Count; r++)
                {
                    if (rows[r][c] == 'O')
                    {
                        int c2 = c + 1;
                        for (; c2 < rows[0].Count; c2++)
                        {
                            if (rows[r][c2] != '.')
                            {
                                break;
                            }
                        }

                        rows[r][c] = '.';
                        rows[r][c2 - 1] = 'O';
                    }
                }
            }
        }

        private static int CountLoad(List<List<char>> platform)
        {
            var totalLoad = 0;
            for (int r = 0; r < platform.Count; r++)
            {
                var rollingStoneLoad = platform.Count - r;
                totalLoad += platform[r].Count(c => c == 'O') * rollingStoneLoad;
            }

            return totalLoad;
        }
    }
}
