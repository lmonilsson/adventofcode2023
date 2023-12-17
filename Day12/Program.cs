

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rows = File.ReadLines("input.txt").Select(Row.Parse).ToList();
            var arrangementsPerRow = rows.Select(r => CountArrangements(r.Springs, r.GroupSizes, 0, 0));
            Console.WriteLine($"Part 1: {arrangementsPerRow.Sum()}");

            // Brute force too slow
            //var unfolded = rows.Select(r => r.Unfold()).ToList();
            //var arrangementsPerUnfoldedRow = unfolded.Select(r => CountArrangements(r.Springs, r.GroupSizes, 0, 0));
            //Console.WriteLine($"Part 2: {arrangementsPerUnfoldedRow.Sum()}");
        }

        private static long CountArrangements(ReadOnlySpan<char> springs, IReadOnlyList<int> groupSizes, int groupIdx, int startPos)
        {
            int groupSize = groupSizes[groupIdx];
            long count = 0;

            for (int i = startPos; i <= springs.Length - groupSize; i++)
            {
                if (!AnyDamaged(springs.Slice(startPos, i - startPos))
                    && AllDamagedOrUnknown(springs.Slice(i, groupSize))
                    && (i + groupSize == springs.Length || IsOperationalOrUnknown(springs[i + groupSize]))
                    && (groupIdx < groupSizes.Count - 1 || !AnyDamaged(springs.Slice(i + groupSize))))
                {
                    if (groupIdx < groupSizes.Count - 1)
                    {
                        count += CountArrangements(springs, groupSizes, groupIdx + 1, i + groupSize + 1);
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static bool AnyDamaged(ReadOnlySpan<char> s)
        {
            return s.Contains('#');
        }

        private static bool AllDamagedOrUnknown(ReadOnlySpan<char> s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != '#' && s[i] != '?')
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsOperationalOrUnknown(char c)
        {
            return c == '.' || c == '?';
        }
    }

    internal record Row(string Springs, IReadOnlyList<int> GroupSizes)
    {
        internal static Row Parse(string line)
        {
            // ??#.?#?#??? 1,3,1
            var sp = line.Split(' ');
            var springs = sp[0];
            var groupSizes = sp[1].Split(',').Select(int.Parse).ToList();
            return new Row(springs, groupSizes);
        }

        internal Row Unfold()
        {
            var unfoldedSprings = string.Join('?', Enumerable.Repeat(Springs, 5));
            var unfoldedGroupSizes = Enumerable.Repeat(GroupSizes, 5).SelectMany(x => x).ToList();
            return new Row(unfoldedSprings, unfoldedGroupSizes);
        }
    }
}
