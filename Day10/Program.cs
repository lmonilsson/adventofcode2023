
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

            var map = File.ReadAllLines("input.txt");
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
        }

        private static (Pos Next, Dir CameFrom) Move(Pos pos, Dir? cameFrom, string[] map)
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

        private static char[] NorthConnecting = new char[] { 'S', '|', 'L', 'J' };
        private static char[] EastConnecting = new char[] { 'S', '-', 'L', 'F' };
        private static char[] SouthConnecting = new char[] { 'S', '|', '7', 'F' };
        private static char[] WestConnecting = new char[] { 'S', '-', 'J', '7' };

        enum Dir
        {
            North,
            East,
            South,
            West
        }

        record Pos(int Row, int Col);
    }
}
