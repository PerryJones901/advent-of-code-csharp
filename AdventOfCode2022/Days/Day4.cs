using AdventOfCode2022.Helpers;

namespace AdventOfCode2022.Days
{
    public static class Day4
    {
        public static int Part1Answer() =>
            GetSeparatedInputFromFile().Select(GetRangesFromInputLine).Count(IsOneRangeFullyNestedInAnother);

        public static int Part2Answer() =>
            GetSeparatedInputFromFile().Select(GetRangesFromInputLine).Count(AreRangesOverlapping);

        public static List<string> GetSeparatedInputFromFile() =>
            FileInputHelper.GetStringListFromFile("Day4.txt");

        public static IEnumerable<Range> GetRangesFromInputLine(string inputLine) =>
            inputLine.Split(',').Select(x => x.Split('-')).Select(x => new Range { Start = int.Parse(x[0]), End = int.Parse(x[1]) });

        public static bool IsOneRangeFullyNestedInAnother(IEnumerable<Range> ranges)
        {
            // We'll assume ranges is always of length 2

            var range1 = ranges.First();
            var range2 = ranges.Skip(1).First();

            return range1.IsFullyContainedIn(range2) || range2.IsFullyContainedIn(range1);
        }

        public static bool AreRangesOverlapping(IEnumerable<Range> ranges)
        {
            // We'll assume ranges is always of length 2
            var range1 = ranges.First();
            var range2 = ranges.Skip(1).First();

            return range1.OverlapsWith(range2) || IsOneRangeFullyNestedInAnother(ranges);
        }

        public class Range
        {
            public int Start { get; set; }
            public int End { get; set; }

            public bool IsFullyContainedIn(Range range) =>
                range.Start <= Start && End <= range.End;

            public bool OverlapsWith(Range range) =>
                (range.Start <= Start && Start <= range.End)
                    || (range.Start <= End && End <= range.End);
        }
    }
}
