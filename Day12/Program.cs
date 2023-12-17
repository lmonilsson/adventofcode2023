

namespace Day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rows = File.ReadLines("input.txt").Select(Row.Parse).ToList();
            var arrangementsPerRow = rows.Select(CountArrangements);
            Console.WriteLine($"Part 1: {arrangementsPerRow.Sum()}");

            var unfolded = rows.Select(r => r.Unfold()).ToList();
            var arrangementsPerUnfoldedRow = unfolded.Select(CountArrangements);
            Console.WriteLine($"Part 2: {arrangementsPerUnfoldedRow.Sum()}");
        }

        private static long CountArrangements(Row row)
        {
            var memo = new Dictionary<(int GroupIdx, int StartPos), long>();
            return CountArrangements(row.Springs, row.GroupSizes, 0, 0, memo);
        }

        private static long CountArrangements(ReadOnlySpan<char> springs, IReadOnlyList<int> groupSizes, int groupIdx, int startPos, Dictionary<(int GroupIdx, int StartPos), long> memo)
        {
            int groupSize = groupSizes[groupIdx];
            long count = 0;

            if (memo.TryGetValue((groupIdx, startPos), out count))
            {
                return count;
            }

            for (int i = startPos; i <= springs.Length - groupSize; i++)
            {
                if (AllDamagedOrUnknown(springs.Slice(i, groupSize))
                    && (i + groupSize == springs.Length || IsOperationalOrUnknown(springs[i + groupSize]))
                    && (groupIdx < groupSizes.Count - 1 || !AnyDamaged(springs.Slice(i + groupSize))))
                {
                    if (groupIdx < groupSizes.Count - 1)
                    {
                        count += CountArrangements(springs, groupSizes, groupIdx + 1, i + groupSize + 1, memo);
                    }
                    else
                    {
                        count++;
                    }
                }

                if (IsDamaged(springs[i]))
                {
                    // This group cannot be matched again at a later index after passing damage,
                    // since the damage would either add to the next matched group or be a separate
                    // group not part of the specification.
                    break;
                }
            }

            memo[(groupIdx, startPos)] = count;

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

        private static bool IsDamaged(char c)
        {
            return c == '#';
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
