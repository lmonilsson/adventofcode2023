
namespace Day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                7-F7-
                .FJ|7
                SJLL7
                |F--J
                LJ.LJ

                | is a vertical pipe connecting north and south.
                - is a horizontal pipe connecting east and west.
                L is a 90-degree bend connecting north and east.
                J is a 90-degree bend connecting north and west.
                7 is a 90-degree bend connecting south and west.
                F is a 90-degree bend connecting south and east.
                . is ground; there is no pipe in this tile.
                S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
             */

            var map = File.ReadAllLines("input.txt")
                .Select((row, r) => row.Select((chr, c) => chr).ToArray())
                .ToArray();
            var loop = Part1(map);
            Part2(map, loop);
            Part2Alt(loop);
        }

        private static List<Pos> Part1(char[][] map)
        {
            var pos = map
                .SelectMany((row, r) => row.Select((chr, c) => (r, c, chr)))
                .Where(rcc => rcc.chr == 'S')
                .Select(rcc => new Pos(rcc.r, rcc.c))
                .First();

            Dir? cameFrom = null;
            var positionsVisited = new List<Pos>();
            do
            {
                (pos, cameFrom) = Move(pos, cameFrom, map);
                positionsVisited.Add(pos);
            }
            while (map[pos.Row][pos.Col] != 'S');

            var furthestSteps = positionsVisited.Count / 2;
            Console.WriteLine($"Part 1: {furthestSteps}");

            return positionsVisited;
        }

        private static void Part2(char[][] map, List<Pos> loop)
        {
            var mapHeight = map.Length;
            var mapWidth = map[0].Length;

            var pendingSpaces = new List<Pos>();
            Pos? edge = null;

            //PrintMap(map, null, null);

            // Remove objects not part of the loop.
            var scrubbedMap = map
                .Select((row, r) => row.Select((chr, c) => chr != '.' && !loop.Contains(Pos.Create(r, c)) ? '.' : chr).ToArray())
                .ToArray();

            //PrintMap(scrubbedMap, null, null);

            // Edge spaces cannot be contained within the loop. Start exploring from those.
            pendingSpaces.AddRange(scrubbedMap
                .SelectMany((row, r) => row.Select((chr, c) => (r, c, chr)))
                .Where(rcc => rcc.chr == '.' && (rcc.r == 0 || rcc.c == 0 || rcc.r == mapHeight - 1 || rcc.c == mapWidth - 1))
                .Select(rcc => new Pos(rcc.r, rcc.c)));

            pendingSpaces.ForEach(pos => scrubbedMap[pos.Row][pos.Col] = ' ');
            var foundEdge = false;
            while (pendingSpaces.Any())
            {
                while (pendingSpaces.Any())
                {
                    var space = pendingSpaces.Last();
                    pendingSpaces.RemoveAt(pendingSpaces.Count - 1);

                    //PrintMap(scrubbedMap, space, null);

                    foreach (var r in new[] { space.Row - 1, space.Row, space.Row + 1})
                    {
                        foreach (var c in new[] { space.Col - 1, space.Col, space.Col + 1 })
                        {
                            if (r >= 0 && r <= mapHeight - 1 && c >= 0 && c <= mapWidth - 1 && scrubbedMap[r][c] == '.')
                            {
                                pendingSpaces.Add(Pos.Create(r, c));
                                scrubbedMap[r][c] = ' ';
                            }
                        }
                    }

                    if (!foundEdge)
                    {
                        if (space.Row > 0 && scrubbedMap[space.Row - 1][space.Col] == MoveNorthLookWest)
                        {
                            edge = space.North();
                        }
                        if (space.Row > 0 && scrubbedMap[space.Row - 1][space.Col] == MoveNorthLookEast)
                        {
                            edge = space.North();
                        }

                        if (space.Row < mapHeight - 1 && scrubbedMap[space.Row + 1][space.Col] == MoveSouthLookWest)
                        {
                            edge = space.South();
                        }
                        if (space.Row > mapHeight - 1 && scrubbedMap[space.Row + 1][space.Col] == MoveSouthLookEast)
                        {
                            edge = space.South();
                        }

                        if (space.Col > 0 && scrubbedMap[space.Row][space.Col - 1] == MoveEastLookNorth)
                        {
                            edge = space.East();
                        }
                        if (space.Col > 0 && scrubbedMap[space.Row][space.Col - 1] == MoveEastLookSouth)
                        {
                            edge = space.East();
                        }

                        if (space.Col < mapWidth - 1 && scrubbedMap[space.Row][space.Col + 1] == MoveWestLookNorth)
                        {
                            edge = space.West();
                        }
                        if (space.Col > mapWidth - 1 && scrubbedMap[space.Row][space.Col + 1] == MoveWestLookSouth)
                        {
                            edge = space.West();
                        }

                        foundEdge = edge != null;
                    }
                }

                //PrintMap(scrubbedMap, edge, null);

                Dir? cameFrom = null;
                Dir? lookDir = null;
                while (edge != null)
                {
                    var symbol = scrubbedMap[edge.Row][edge.Col];
                    var next = TryMove(edge, cameFrom, scrubbedMap);
                    scrubbedMap[edge.Row][edge.Col] = ' ';
                    var movedToward = cameFrom?.Opposite();
                    var movingTowardNext = next?.CameFrom.Opposite();

                    //PrintMap(scrubbedMap, edge, lookDir);

                    if (lookDir != null)
                    {
                        var lookingAt = edge.Move(lookDir.Value);
                        if (lookingAt.Row >= 0 && lookingAt.Row <= mapHeight - 1 && lookingAt.Col >= 0 && lookingAt.Col <= mapWidth - 1
                            && scrubbedMap[lookingAt.Row][lookingAt.Col] == '.')
                        {
                            pendingSpaces.Add(lookingAt);
                            scrubbedMap[lookingAt.Row][lookingAt.Col] = ' ';
                        }
                    }

                    if (next != null)
                    {
                        if (lookDir == null)
                        {
                            switch (movingTowardNext)
                            {
                                case Dir.South:
                                    if (symbol == '7')
                                        lookDir = Dir.East;
                                    else
                                        lookDir = Dir.West;
                                    break;
                                case Dir.West:
                                    if (symbol == 'J')
                                        lookDir = Dir.North;
                                    else
                                        lookDir = Dir.South;
                                    break;
                                case Dir.North:
                                    if (symbol == 'L')
                                        lookDir = Dir.West;
                                    else
                                        lookDir = Dir.East;
                                    break;
                                case Dir.East:
                                    if (symbol == 'L')
                                        lookDir = Dir.South;
                                    else
                                        lookDir = Dir.North;
                                    break;
                                default:
                                    throw new Exception("Failed to determine initial look direction");
                            }
                        }
                        else
                        {
                            switch (movedToward)
                            {
                                case Dir.South:
                                    lookDir = movingTowardNext switch
                                    {
                                        Dir.West => lookDir.Value.RotateRight(),
                                        Dir.East => lookDir.Value.RotateLeft(),
                                        _ => lookDir
                                    };
                                    break;
                                case Dir.West:
                                    lookDir = movingTowardNext switch
                                    {
                                        Dir.South => lookDir.Value.RotateLeft(),
                                        Dir.North => lookDir.Value.RotateRight(),
                                        _ => lookDir
                                    };
                                    break;
                                case Dir.North:
                                    lookDir = movingTowardNext switch
                                    {
                                        Dir.West => lookDir.Value.RotateLeft(),
                                        Dir.East => lookDir.Value.RotateRight(),
                                        _ => lookDir
                                    };
                                    break;
                                case Dir.East:
                                    lookDir = movingTowardNext switch
                                    {
                                        Dir.South => lookDir.Value.RotateRight(),
                                        Dir.North => lookDir.Value.RotateLeft(),
                                        _ => lookDir
                                    };
                                    break;
                            }
                        }

                        if (lookDir != null)
                        {
                            var lookingAt = edge.Move(lookDir.Value);
                            if (lookingAt.Row >= 0 && lookingAt.Row <= mapHeight - 1 && lookingAt.Col >= 0 && lookingAt.Col <= mapWidth - 1
                                && scrubbedMap[lookingAt.Row][lookingAt.Col] == '.')
                            {
                                pendingSpaces.Add(lookingAt);
                                scrubbedMap[lookingAt.Row][lookingAt.Col] = ' ';
                            }
                        }

                        //PrintMap(scrubbedMap, next.Value.Next, lookDir);

                        edge = next.Value.Next;
                        cameFrom = next.Value.CameFrom;
                    }
                    else
                    {
                        edge = null;
                    }
                }
            }

            //PrintMap(scrubbedMap, null, null);
            var tilesWithinLoop = scrubbedMap.SelectMany(row => row).Count(x => x == '.');
            Console.WriteLine($"Part 2: {tilesWithinLoop}");
        }

        private static void Part2Alt(List<Pos> loop)
        {
            // Shoelace formula
            int a = 0;
            for (int i = 0; i < loop.Count; i++)
            {
                a += loop[i].Col * loop[(i + 1) % loop.Count].Row;
                a -= loop[i].Row * loop[(i + 1) % loop.Count].Col;
            }
            a /= 2;

            // Pick's theorem
            // A = i + (b/2) - 1
            // i = A - (b/2) + 1
            int points = a - (loop.Count / 2) + 1;
            Console.WriteLine($"Part 2 alt: {points}");
        }

        private static void PrintMap(char[][] map, Pos? curPos, Dir? looking)
        {
            for (int r = 0; r < map.Length; r++)
            {
                for (int c = 0; c < map[0].Length; c++)
                {
                    if (r == curPos?.Row && c == curPos?.Col)
                    {
                        switch (looking)
                        {
                            case Dir.North:
                                Console.Write('^');
                                break;
                            case Dir.East:
                                Console.Write('>');
                                break;
                            case Dir.South:
                                Console.Write('v');
                                break;
                            case Dir.West:
                                Console.Write('<');
                                break;
                            case null:
                                Console.Write('*');
                                break;
                        }
                    }
                    else
                    {
                        Console.Write(map[r][c]);
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static (Pos Next, Dir CameFrom) Move(Pos pos, Dir? cameFrom, char[][] map)
        {
            if (cameFrom != Dir.North &&
                pos.Row > 0 &&
                NorthConnecting.Contains(map[pos.Row][pos.Col]) &&
                SouthConnecting.Contains(map[pos.Row - 1][pos.Col]))
            {
                return (new Pos(pos.Row - 1, pos.Col), Dir.South);
            }

            if (cameFrom != Dir.East &&
                pos.Col < map[0].Length &&
                EastConnecting.Contains(map[pos.Row][pos.Col]) &&
                WestConnecting.Contains(map[pos.Row][pos.Col + 1]))
            {
                return (new Pos(pos.Row, pos.Col + 1), Dir.West);
            }

            if (cameFrom != Dir.South &&
                pos.Row < map.Length &&
                SouthConnecting.Contains(map[pos.Row][pos.Col]) &&
                NorthConnecting.Contains(map[pos.Row + 1][pos.Col]))
            {
                return (new Pos(pos.Row + 1, pos.Col), Dir.North);
            }

            if (cameFrom != Dir.West &&
                pos.Col > 0 &&
                WestConnecting.Contains(map[pos.Row][pos.Col]) &&
                EastConnecting.Contains(map[pos.Row][pos.Col - 1]))
            {
                return (new Pos(pos.Row, pos.Col - 1), Dir.East);
            }

            throw new Exception("Failed to make a move!");
        }


        private static (Pos Next, Dir CameFrom)? TryMove(Pos pos, Dir? cameFrom, char[][] map)
        {
            if (cameFrom != Dir.North &&
                pos.Row > 0 &&
                NorthConnecting.Contains(map[pos.Row][pos.Col]) &&
                SouthConnecting.Contains(map[pos.Row - 1][pos.Col]))
            {
                return (new Pos(pos.Row - 1, pos.Col), Dir.South);
            }

            if (cameFrom != Dir.East &&
                pos.Col < map[0].Length &&
                EastConnecting.Contains(map[pos.Row][pos.Col]) &&
                WestConnecting.Contains(map[pos.Row][pos.Col + 1]))
            {
                return (new Pos(pos.Row, pos.Col + 1), Dir.West);
            }

            if (cameFrom != Dir.South &&
                pos.Row < map.Length &&
                SouthConnecting.Contains(map[pos.Row][pos.Col]) &&
                NorthConnecting.Contains(map[pos.Row + 1][pos.Col]))
            {
                return (new Pos(pos.Row + 1, pos.Col), Dir.North);
            }

            if (cameFrom != Dir.West &&
                pos.Col > 0 &&
                WestConnecting.Contains(map[pos.Row][pos.Col]) &&
                EastConnecting.Contains(map[pos.Row][pos.Col - 1]))
            {
                return (new Pos(pos.Row, pos.Col - 1), Dir.East);
            }

            return null;
        }

        private static char[] NorthConnecting = new char[] { 'S', '|', 'L', 'J' };
        private static char[] EastConnecting = new char[] { 'S', '-', 'L', 'F' };
        private static char[] SouthConnecting = new char[] { 'S', '|', '7', 'F' };
        private static char[] WestConnecting = new char[] { 'S', '-', 'J', '7' };

        private static char[] Bends = new char[] { 'J', '7', 'F', 'L' };

        private const char MoveEastLookNorth = 'F';
        private const char MoveEastLookSouth = 'L';
        private const char MoveWestLookNorth = '7';
        private const char MoveWestLookSouth = 'J';
        private const char MoveNorthLookEast = 'J';
        private const char MoveNorthLookWest = 'L';
        private const char MoveSouthLookEast = '7';
        private const char MoveSouthLookWest = 'F';
    }

    internal enum Dir
    {
        North,
        East,
        South,
        West
    }

    internal static class DirMethods
    {
        public static Dir RotateLeft(this Dir dir)
        {
            return dir switch
            {
                Dir.North => Dir.West,
                Dir.East => Dir.North,
                Dir.South => Dir.East,
                Dir.West => Dir.South,
                _ => throw new Exception("Invalid dir")
            };
        }

        public static Dir RotateRight(this Dir dir)
        {
            return dir switch
            {
                Dir.North => Dir.East,
                Dir.East => Dir.South,
                Dir.South => Dir.West,
                Dir.West => Dir.North,
                _ => throw new Exception("Invalid dir")
            };
        }

        public static Dir Opposite(this Dir dir)
        {
            return dir switch
            {
                Dir.North => Dir.South,
                Dir.East => Dir.West,
                Dir.South => Dir.North,
                Dir.West => Dir.East,
                _ => throw new Exception("Invalid dir")
            };
        }
    }

    internal record Pos(int Row, int Col)
    {
        public static Pos Create(int row, int col)
        {
            return new Pos(row, col);
        }

        public Pos North()
        {
            return Pos.Create(Row - 1, Col);
        }

        public Pos South()
        {
            return Pos.Create(Row + 1, Col);
        }

        public Pos West()
        {
            return Pos.Create(Row, Col - 1);
        }

        public Pos East()
        {
            return Pos.Create(Row, Col + 1);
        }

        public Pos Move(Dir dir)
        {
            switch (dir)
            {
                case Dir.North:
                    return North();
                case Dir.East:
                    return East();
                case Dir.South:
                    return South();
                case Dir.West:
                    return West();
                default:
                    throw new ArgumentException("Invalid direction", nameof(dir));
            }
        }
    };
}
